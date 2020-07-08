﻿using System;
using System.Collections.Generic;
using Endscript.Helpers;
using Endscript.Commands;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using CoreExtensions.Management;



namespace Endscript.Core
{
	public class EndScriptManager
	{
		private readonly List<EndError> _errors;
		private readonly CollectionMap _map;
		private readonly BaseCommand[] _commands;
		private readonly Stack<ISelectable> _stack;
		private int _index = -1;
		private bool _waiting_selection;

		public IEnumerable<EndError> Errors => this._errors;
		public int CurrentIndex => this._index;
		public BaseCommand CurrentCommand
		{
			get
			{
				if (this._commands is null || this._index < 0) return null;
				else if (this._index >= this._commands.Length) return null;
				return this._commands[this._index];
			}
		}

		public EndScriptManager(BaseProfile profile, BaseCommand[] commands, string launcher)
		{
			this._errors = new List<EndError>();
			this._stack = new Stack<ISelectable>();
			this._commands = commands;
			this._map = new CollectionMap(profile, launcher);
			this._index = 0;
		}

		~EndScriptManager()
		{
			#if DEBUG
			Console.WriteLine("EndScriptManager destroyed");
			#endif
		}

		/// <summary>
		/// Processes commands passed on initialization. When returns <see cref="true"/> that 
		/// means all commands have been executed without errors. When returns <see cref="false"/> 
		/// that means <see cref="EndScriptManager"/> is waiting for user input in the current 
		/// <see cref="ISelectable"/> command. When user makes selection, the method has to be 
		/// called again in order to continue execution.
		/// </summary>
		/// <returns></returns>
		public bool ProcessScript()
		{
			try
			{

				while (this._index < this._commands.Length)
				{

					var command = this._commands[this._index];

					if (command is EndCommand end)
					{

						if (this._stack.Count > 0)
						{

							this._stack.Pop(); // if stack is not empty

						}
						else
						{

							throw new RuntimeAnalysisException("Unexpected 'end' command", command.Filename, command.Index);

						}

					}

					else if (command is ISelectable select) // if selectable
					{

						// We have to make user make selection by returnin false;
						// once user calls function back, we can continue executing
						if (select is IfStatementCommand ifstate) ifstate.Execute(this._map);
						else if (this._waiting_selection) this._waiting_selection = false;
						else { this._waiting_selection = true; return false; }

						this._stack.Push(select); // set selectable to find

						while (this._index < this._commands.Length) // bound it
						{

							// traverse till we find matching option
							var next = this._commands[++this._index]; // get next command

							if (next is OptionalCommand optional && // if matches, break
								select.Choice == select.ParseOption(optional.Option))
							{

								break;

							}

						}

					}

					else if (this._stack.Count > 0 && command is OptionalCommand optional) // if optional command
					{

						var peek = this._stack.Peek(); // get last ISelectable

						if (peek.Contains(optional.Option)) // if contains
						{

							while (this._index < this._commands.Length) // bound it
							{

								// we traverse till we find end command
								var next = this._commands[++this._index];

								if (next is EndCommand final)
								{

									this._stack.Pop(); // pop from the stack
									break;

								}

							}

						}
						else this.ExecuteSingle(command);

					}

					else this.ExecuteSingle(command);

					++this._index;

				}

				if (this._stack.Count == 0) return true;
				else throw new RuntimeAnalysisException("Closing 'end' command was never found");

			}
			catch (RuntimeAnalysisException runtime) { throw runtime; }
			catch (IndexOutOfRangeException) { throw new Exception("Unable to find end to a selectable statement"); }
			catch (Exception ex) { throw ex; }
		}

		private void ExecuteSingle(BaseCommand command)
		{
			try
			{

				command.Execute(this._map);

				#if DEBUG
				Console.WriteLine($"Executing [{command.Line}]");
				#endif

			}
			catch (Exception e)
			{

				this._errors.Add(new EndError()
				{
					Error = e.GetLowestMessage(),
					Filename = command.Filename,
					Line = command.Line,
					Index = command.Index,
				});

			}
		}
	}
}
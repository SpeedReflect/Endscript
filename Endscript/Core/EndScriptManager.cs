using System;
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
		private bool _stop_errors;

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
			this.CommandChase();
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
						var option = select.Options[select.Choice];
						this._index = option.Start;
						if (this._index == -1) throw new Exception($"Missing optional command '{option.Name}'");

					}

					else if (this._stack.Count > 0 && command is OptionalCommand optional) // if optional command
					{

						var peek = this._stack.Peek(); // get last ISelectable

						if (peek.Contains(optional.Option))
						{

							if (peek.LastCommand == -1) throw new IndexOutOfRangeException();
							this._index = peek.LastCommand;
							this._stack.Pop();

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

		private void CommandChase()
		{
			var jumpstack = new Stack<ISelectable>();

			for (int i = 0; i < this._commands.Length; ++i)
			{

				var command = this._commands[i];

				if (command is ISelectable selectable)
				{

					jumpstack.Push(selectable);
					continue;

				}

				else if (command is OptionalCommand optional)
				{

					var peek = jumpstack.Peek();
					if (peek.Contains(optional.Option)) peek[optional.Option].Start = i;

				}

				else if (command is EndCommand end)
				{

					jumpstack.Peek().LastCommand = i;
					jumpstack.Pop();

				}

			}
		}

		private void ExecuteSingle(BaseCommand command)
		{
			try
			{

				//command.Execute(this._map);

				if (command is StopErrorsCommand stop)
				{

					this._stop_errors = stop.Enable;

				}

				#if DEBUG
				Console.WriteLine($"Executing [{command.Line}]");
				#endif

			}
			catch (Exception e)
			{

				if (!this._stop_errors)
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
}

using System; 

using System.Collections.Generic; 

 

namespace AdvancedCalculator 

{ 

    class Program 

    { 

        static void Main(string[] args) 

        { 

            Calculator calculator = new Calculator(); 

            calculator.Run(); 

        } 

    } 

 

    class Calculator 

    { 

        private Dictionary<string, Func<double, double, double>> _basicOperations; 

        private Dictionary<string, Func<double, double>> _scientificOperations; 

        private List<string> _history; 

        private double _memory; 

        private bool _running; 

 

        public Calculator() 

        { 

 

            _basicOperations = new Dictionary<string, Func<double, double, double>> 

            { 

                { "+", (a, b) => a + b }, 

                { "-", (a, b) => a - b }, 

                { "*", (a, b) => a * b }, 

                { "/", (a, b) => a / b }, 

                { "%", (a, b) => a % b }, 

                { "^", (a, b) => Math.Pow(a, b) } 

            }; 

 

            _scientificOperations = new Dictionary<string, Func<double, double>> 

            { 

                { "sqrt", a => Math.Sqrt(a) }, 

                { "sin", a => Math.Sin(a) }, 

                { "cos", a => Math.Cos(a) }, 

                { "tan", a => Math.Tan(a) }, 

                { "log", a => Math.Log10(a) }, 

                { "ln", a => Math.Log(a) } 

            }; 

 

            _history = new List<string>(); 

            _memory = 0; 

            _running = true; 

        } 

 

        public void Run() 

        { 

            DisplayWelcomeMessage(); 

 

            while (_running) 

            { 

                try 

                { 

                    DisplayMenu(); 

                    string input = Console.ReadLine().Trim().ToLower(); 

                    ProcessInput(input); 

                } 

                catch (Exception ex) 

                { 

                    Console.ForegroundColor = ConsoleColor.Red; 

                    Console.WriteLine($"Error: {ex.Message}"); 

                    Console.ResetColor(); 

                } 

 

                Console.WriteLine("\nPress any key to continue..."); 

                Console.ReadKey(); 

                Console.Clear(); 

            } 

 

            Console.WriteLine("Thank you for using the Advanced Calculator. Bubyee!"); 

        } 

 

        private void DisplayWelcomeMessage() 

        { 

            Console.ForegroundColor = ConsoleColor.Cyan; 

            Console.WriteLine("======================================"); 

            Console.WriteLine("      ADVANCED CONSOLE CALCULATOR     "); 

            Console.WriteLine("======================================"); 

            Console.ResetColor(); 

            Console.WriteLine("Enter calculations directly or use commands:"); 

            Console.WriteLine("Examples: '5 + 3', '10 * 2', 'sqrt 16'"); 

            Console.WriteLine(); 

        } 

 

        private void DisplayMenu() 

        { 

            Console.ForegroundColor = ConsoleColor.Yellow; 

            Console.WriteLine("OPTIONS:"); 

            Console.ResetColor(); 

            Console.WriteLine("  - Direct calculation: 'number operator number' (e.g., '5 + 3')"); 

            Console.WriteLine("  - Scientific functions: 'function number' (e.g., 'sqrt 16')"); 

            Console.WriteLine("  - 'history': Show calculation history"); 

            Console.WriteLine("  - 'clear': Clear history"); 

            Console.WriteLine("  - 'memory': Display stored memory value"); 

            Console.WriteLine("  - 'm+': Add last result to memory"); 

            Console.WriteLine("  - 'm-': Subtract last result from memory"); 

            Console.WriteLine("  - 'mc': Clear memory"); 

            Console.WriteLine("  - 'help': Show available operations"); 

            Console.WriteLine("  - 'exit': Quit calculator"); 

            Console.Write("\nEnter calculation or command: "); 

        } 

 

        private void ProcessInput(string input) 

        { 

            if (string.IsNullOrWhiteSpace(input)) 

                return; 

 

           

            switch (input) 

            { 

                case "exit": 

                    _running = false; 

                    return; 

                case "history": 

                    DisplayHistory(); 

                    return; 

                case "clear": 

                    _history.Clear(); 

                    Console.WriteLine("History cleared."); 

                    return; 

                case "memory": 

                    Console.WriteLine($"Memory value: {_memory}"); 

                    return; 

                case "m+": 

                    if (_history.Count > 0) 

                    { 

                        string lastResult = _history[_history.Count - 1].Split('=')[1].Trim(); 

                        if (double.TryParse(lastResult, out double value)) 

                        { 

                            _memory += value; 

                            Console.WriteLine($"Added {value} to memory. Memory = {_memory}"); 

                        } 

                    } 

                    return; 

                case "m-": 

                    if (_history.Count > 0) 

                    { 

                        string lastResult = _history[_history.Count - 1].Split('=')[1].Trim(); 

                        if (double.TryParse(lastResult, out double value)) 

                        { 

                            _memory -= value; 

                            Console.WriteLine($"Subtracted {value} from memory. Memory = {_memory}"); 

                        } 

                    } 

                    return; 

                case "mc": 

                    _memory = 0; 

                    Console.WriteLine("Memory cleared."); 

                    return; 

                case "help": 

                    DisplayHelp(); 

                    return; 

            } 

 

            foreach (var op in _scientificOperations.Keys) 

            { 

                if (input.StartsWith(op + " ")) 

                { 

                    string numberStr = input.Substring(op.Length).Trim(); 

                     

                    if (numberStr == "m") 

                    { 

                        numberStr = _memory.ToString(); 

                    } 

                     

                    if (double.TryParse(numberStr, out double number)) 

                    { 

                        double result = _scientificOperations[op](number); 

                        string operation = $"{op}({number}) = {result}"; 

                        _history.Add(operation); 

                         

                        Console.ForegroundColor = ConsoleColor.Green; 

                        Console.WriteLine(operation); 

                        Console.ResetColor(); 

                        return; 

                    } 

                } 

            } 

 

            string[] parts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); 

             

            if (parts.Length == 3) 

            { 

                string leftStr = parts[0]; 

                string op = parts[1]; 

                string rightStr = parts[2]; 

                 

                if (leftStr == "m") 

                { 

                    leftStr = _memory.ToString(); 

                } 

                if (rightStr == "m") 

                { 

                    rightStr = _memory.ToString(); 

                } 

                 

                if (_basicOperations.ContainsKey(op) &&  

                    double.TryParse(leftStr, out double left) &&  

                    double.TryParse(rightStr, out double right)) 

                { 

                    try 

                    { 

                        double result = _basicOperations[op](left, right); 

                        string operation = $"{left} {op} {right} = {result}"; 

                        _history.Add(operation); 

                         

                        Console.ForegroundColor = ConsoleColor.Green; 

                        Console.WriteLine(operation); 

                        Console.ResetColor(); 

                        return; 

                    } 

                    catch (Exception ex) 

                    { 

                        throw new Exception($"Calculation error: {ex.Message}"); 

                    } 

                } 

            } 

 

     

            Console.ForegroundColor = ConsoleColor.Red; 

            Console.WriteLine("Invalid input. Try again or type 'help' for guidance."); 

            Console.ResetColor(); 

        } 

 

        private void DisplayHistory() 

        { 

            if (_history.Count == 0) 

            { 

                Console.WriteLine("No calculations in history."); 

                return; 

            } 

 

            Console.ForegroundColor = ConsoleColor.Magenta; 

            Console.WriteLine("Calculation History:"); 

            Console.ResetColor(); 

             

            for (int i = 0; i < _history.Count; i++) 

            { 

                Console.WriteLine($"{i + 1}. {_history[i]}"); 

            } 

        } 

 

        private void DisplayHelp() 

        { 

            Console.ForegroundColor = ConsoleColor.Cyan; 

            Console.WriteLine("Available Operations:"); 

            Console.ResetColor(); 

             

            Console.WriteLine("Basic Operations:"); 

            foreach (var op in _basicOperations.Keys) 

            { 

                Console.WriteLine($"  {op}"); 

            } 

             

            Console.WriteLine("\nScientific Functions:"); 

            foreach (var op in _scientificOperations.Keys) 

            { 

                Console.WriteLine($"  {op}"); 

            } 

             

            Console.WriteLine("\nMemory Operations:"); 

            Console.WriteLine("  memory - Display current memory value"); 

            Console.WriteLine("  m+ - Add last result to memory"); 

            Console.WriteLine("  m- - Subtract last result from memory"); 

            Console.WriteLine("  mc - Clear memory"); 

            Console.WriteLine("  Use 'm' in calculations to refer to memory value"); 

             

            Console.WriteLine("\nOther Commands:"); 

            Console.WriteLine("  history - Show calculation history"); 

            Console.WriteLine("  clear - Clear history"); 

            Console.WriteLine("  exit - Quit calculator"); 

        } 

    } 

} 

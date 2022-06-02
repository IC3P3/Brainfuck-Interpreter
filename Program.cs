namespace brainfuck {
    class brainfuck {
        static void Main(string[] args) {
            // if a path is in the arguments, use that path
            // otherwise ask for a path 

            if(args.Length > 0) {
                checkCode(args[0]);
            } else {
                getPath();
            }
        }

        static void getPath() {
            Console.WriteLine("Enter path to a brainfuck file: ");
            string path = Console.ReadLine();
            checkCode(path);
        }

        static void checkCode(string filePath) {
            // throw error if file doesn't exist
            if (!File.Exists(filePath)) {
                Console.WriteLine("File does not exist");
                Console.ReadKey(true);
                return;
            }

            // get file extension of file in path
            string extension = Path.GetExtension(filePath);

            // if file is not a brainfuck file, throw error
            if (extension != ".bf" && extension != ".b") {
                Console.WriteLine("File is not a brainfuck file");
                Console.ReadKey(true);
                return;
            }

            // read file into a char array
            char[] code = File.ReadAllText(filePath).ToCharArray();

            // go through all chars in code and check if every [ has a ] and every ] has a [
            bool valid = true;
            int brackets = 0;
            for (int i = 0; i < code.Length; i++) {
                if (code[i] == '[') {
                    brackets++;
                } else if (code[i] == ']') {
                    brackets--;
                }

                if (brackets < 0) {
                    valid = false;
                    break;
                }
            }

            // if brackets are not balanced, throw error
            if (!valid || brackets != 0) {
                Console.WriteLine("Brackets are not balanced");
                Console.ReadKey(true);
                return;
            }

            execute(code);
        }

        static void execute(char[] code) {
            // initialize the memory array and pointer
            sbyte[] memory = new sbyte[30000];
            int pointer = 0;

            // loop through the code
            for (int i = 0; i < code.Length; i++) {
                switch (code[i]) {
                    case '>':
                        pointer++;
                        break;
                    case '<':
                        pointer--;
                        break;
                    case '+':
                        memory[pointer]++;
                        break;
                    case '-':
                        memory[pointer]--;
                        break;
                    case '.':
                        Console.Write(Convert.ToChar(memory[pointer]));
                        break;
                    case ',':
                        memory[pointer] = Convert.ToSByte(Console.Read());
                        break;
                    case '[':
                        if(memory[pointer] == 0) {
                            int depth = 0;
                            for (int j = i; j < code.Length; j++) {
                                if (code[j] == '[') {
                                    depth++;
                                } else if (code[j] == ']') {
                                    depth--;
                                }
                                if (depth == 0) {
                                    i = j;
                                    break;
                                }
                            }
                        }
                        break;
                    case ']':
                        if(memory[pointer] != 0) {
                            int depth = 0;
                            for (int j = i; j >= 0; j--) {
                                if (code[j] == ']') {
                                    depth++;
                                } else if (code[j] == '[') {
                                    depth--;
                                }
                                if (depth == 0) {
                                    i = j;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            
            Console.ReadKey(true);
        }
    }
}
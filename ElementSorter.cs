/* Christian Kenny
 * June 8th 2017
 * This program will take the input of chemical compounds by the user, 
 * and output the individual elements on a cmd prompt
 */

using System;
using System.Collections.Generic;
namespace ElementSorter{
   class Program{

        /*enumerator list that will be used throughout the program to determine the difference
          between upper and lower case letters, as well as numbers and paretheses  */
        public enum interpret_text { uppercase, lowercase, numbers, parentheses}; 


        public class user_input{
            //Get and Set proporties for user input 
            public int numberValue { get; set; }
            public string stringValue { get; set; }
            public interpret_text elementText { get; set; }

            //Reads user input to see if elements selected are letters, numbers, or parentheses
            public user_input (string users_input){
                stringValue = users_input;
                if (Char.IsLetter(Char.Parse(users_input))){  //Checks for letters, recognizes upper and lower case
                
                    elementText = (Char.IsUpper(Char.Parse(users_input))) ? interpret_text.uppercase : interpret_text.lowercase; //null operator
                    if (Char.IsUpper(Char.Parse(users_input))){
                        numberValue = 1; //If no number, assume that there is one atom when given an input, instead of 0
                    }
                }else if (Char.IsDigit(Char.Parse(users_input))){ //checks for numbers
                        elementText = interpret_text.numbers;
                        numberValue = int.Parse(users_input);
                }else{
                    elementText = interpret_text.parentheses; //anything else must be parentheses, otherwise not a valid input
                }
            }
            
        }
        //Main method
        static void Main(string[] args){
            while (true) // Allows the user to continue to input elements without the cmd prompt closing
            {
                
                Console.Write("Enter a chemical compound like H2O, NaHCO3, or C2HF3O2: ");
                string inputString = Console.ReadLine();
                List<user_input> Elements = new List<user_input>();

               
                foreach (var elementID in inputString){
                    Elements.Add(new user_input(elementID.ToString())); //User input becomes a list for counting through
                }


                try{ //Try catch block to intercept user error, usually by not inputting proper compounds
                    //large for loop to count each part of user input and indentify it
                    for (int i = 0; i < Elements.Count; i++){
                        if (Elements[i].elementText == interpret_text.uppercase){
                            if (i == (Elements.Count - 1)){
                                break;
                            }
                            switch (Elements[i + 1].elementText){ //Different cases based on input and how to handle them
              
                                //case for lowercase input
                                case interpret_text.lowercase:
                                    Elements[i].stringValue =
                                    Elements[i].stringValue + Elements[i + 1].stringValue; //Properly combines Upper and lower case letters 
                                    Elements.Remove(Elements[i + 1]); //Removes previous lowercase letter, as upper and lowercase input is now one piece

                                    if (Elements[i + 1].elementText == interpret_text.numbers){
                                        goto case interpret_text.numbers; //switch case
                                    }
                                    break;

                                //case for numbers
                                case interpret_text.numbers:
                                    if ((i + 2 < Elements.Count) && (Elements[i + 2].elementText == interpret_text.numbers)){
                                        Elements[i + 1].stringValue =
                                        Elements[i + 1].stringValue + Elements[i + 2].stringValue;

                                        Elements[i + 1].numberValue =
                                        int.Parse(Elements[i + 1].stringValue);
                                        Elements.Remove(Elements[i + 2]);
                                    }

                                    Elements[i].numberValue = 
                                    Elements[i].numberValue * Elements[i + 1].numberValue;
                                    Elements.Remove(Elements[i + 1]);
                                    break;

                                default: //breaks switch / case statements
                                    break;

                            }
                        }
                        else if (Elements[i].elementText == interpret_text.parentheses)
                        { //If not letters or numbers, must be parentheses
                            int startIndex = i;
                            int endIndex = i + 2;
                            while (Elements[endIndex].stringValue != ")"){
                                endIndex++;
                            }
                            for (int j = startIndex + 1; j < endIndex; j++){
                                Elements[j].numberValue =
                                Elements[j].numberValue * Elements[endIndex + 1].numberValue;
                            }
                            //Removes items in the list
                            Elements.RemoveAt(startIndex);
                            Elements.RemoveAt(endIndex);
                            Elements.RemoveAt(endIndex + 1);
                        }
                    }
                }
                catch (Exception e){
                    Console.WriteLine("{0} Exception caught.", e);
                }

                //For loop recognize input and separates it
                for (int i = 0; i < 2; i++){
                    int list_position = 0;
                    var text = new List<string>();
                    foreach (var element in Elements){
                        if (text.Contains(element.stringValue)){
                            Elements[text.IndexOf(element.stringValue)].numberValue =
                            Elements[text.IndexOf(element.stringValue)].numberValue +
                            element.numberValue;

                            Elements.Remove(element);
                        }
                        else{
                            text.Add(element.stringValue);
                        }
                        list_position++;
                        if (Elements.Count <= list_position){
                            break;
                        }
                    }
                }
                foreach (var element in Elements){ //Writes the new output to the console
                    Console.WriteLine("The element symbol is: " + element.stringValue + ". The number of atoms: " + element.numberValue);
                }
            }
        }
    }  
}
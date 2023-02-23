/*AUTHOR: Carlos Gomez
 DATE: 12/12/2022
 DESCRIPTION: Aquesta es la meva versió del famós joc Wordle. Projecto troncal de la M03, UF1, UF2 i UF3 */

namespace Wordlee;
public class Wordle
{
    public static void Main()
    {
        Wordle p = new Wordle();
        p.Menu();

    }

    void Menu()
    {
        string select;
        string lang = "ESP";

        do
        {
            Console.WriteLine("1. Jugar partida ");
            Console.WriteLine("2. Idioma");
            Console.WriteLine("3. Registro victorias");
            Console.WriteLine("4. Salir");

            select = Console.ReadLine();

            switch (select)
            {
                case "1":
                    Game();
                    break;
                case "2":

                    break;
                case "3":

                    break;
                case "4":

                    break;

                case "9":
                    Accentos();
                    break;
                default:
                    Console.WriteLine("Opcion no reconocida");
                    break;
            }

        } while (select != "end");
    }

    void Game()
    {


        string pathWords = @"..\..\..\palabrasSin.txt";
        string palabraClave = GetWord(pathWords);

        bool win = false;
        int numIntentos = 6;
        bool start = false;
        string palabra = "";//declaro l'string que utilitzare

        palabraClave = palabraClave.ToLower();//poso a majuscules les paraules clau 

       
        List<string> palabrasIntroducidas = new List<string>();
        ConsoleColor[,] listOfColors = new ConsoleColor[numIntentos, 5];
        List<string[]> list = new List<string[]>();

        // string[] palabrasIntrocidas = new string[6];//les array de paraules introduides


        do//aquest for fa de contador
        {
            palabrasIntroducidas.RemoveRange(0, palabrasIntroducidas.Count);

            for (int i = 0; i < numIntentos && palabra != "end"; i++)
            {
                if(start == true){

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\welcome.txt"));                                                                                                        
                    Console.ResetColor();
                    PrintOthersWords(palabrasIntroducidas, palabraClave, listOfColors,list);
                }
                else
                {
                    Console.WriteLine(palabraClave);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\welcome.txt"));
                    Console.ResetColor();
                    start = true;

                }

                do
                {
                    Console.WriteLine("Donam una paraula amb 5 lletres \n ");
                    Console.WriteLine(palabraClave);
                    palabra = Console.ReadLine().ToLower();//demano una paraula
                }
                while (palabraClave.Length != palabra.Length && palabra != "end");//es fa un bucle mentres la paraulaClau no sigui igual a la paraula introduida

                palabrasIntroducidas.Add(palabra);//la palabra introducida en este count (turno) es igual a la palabra

                if (palabra == palabraClave)
                {
                    win = true;
                    palabra = "end";
                }
                else if (i == numIntentos - 1) palabra = "end";

            }

        } while (palabra != "end");

        Victoria(win, palabraClave);
    }
    bool Victoria(bool win, string palabraClave)
    {

        if (win)//si las palabras son iguales
        {
            Console.WriteLine("¡Felicidades, has ganado!");
            PrintWord(palabraClave, ConsoleColor.Green);
            Console.ReadLine();
        }
        else // si les paraules son iguals i ja han pasat tots el torns
        {
            PrintWord(palabraClave, ConsoleColor.Red);
            Console.WriteLine("¡Has perdido!");
            Console.ReadLine();
        }
        Console.Clear();//clean

        return win;
    }

    /// <summary>
    /// Escoge una palabra aleatoria del archivo
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string GetWord(string path)
    {
        //path = @"..\..\..\palabras.txt";

        List<string> words = new List<string>();
        Random rnd = new Random();

        int num;

        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                words.Add(line);
            }
        }

        num = rnd.Next(0, words.Count + 1);

        return words[num];
    }


   /* string[] GetLetterFile(char letra)
    {
        string pathFolderLetters = @"..\..\..\letters\";
        string letters = File.ReadAllText(pathFolderLetters + letra + ".txt");

        string[] letter = letters.Split('\n');

        return letter;
    }*/
    void Print(char letra, ConsoleColor color)
    {
        string pathFolderLetters = @"..\..\..\letters\";
        string letter = File.ReadAllText(pathFolderLetters + letra + ".txt");

        Console.ForegroundColor = color;

        Console.Write(letter);
        Console.ResetColor();

    }

    void PrintWord(string palabra, ConsoleColor color)
    {

        Console.ForegroundColor = color;
        Console.Write(palabra + "\t");
        Console.ResetColor();
    }


    void PrintOthersWords(List<string> palabrasIntroducidas, string palabraClave, ConsoleColor[,] listOfColors, List<string[]> list)
    {
       
        bool presente;

        for (int i = 0; i < palabrasIntroducidas.Count; i++)//aixo ho utilitzare per printar totes les paraules que mha donat l'usuari, per aixo el delimitador del for es el count dels torns
        {
            Console.WriteLine();
            string letraCompr = palabrasIntroducidas[i];//escolleixo de l'array de paraules introduides
            for (int j = 0; j < palabraClave.Length; j++)//aquest for l'utilitzare per comparar una posicio en concret de la paraula introduida
            {

                presente = false;//igualo presente a false

                if (letraCompr[j] == palabraClave[j])
                {
                    listOfColors[i, j] = ConsoleColor.Green;
                    //Print(letraCompr[j], ConsoleColor.Green);//printo la lletra de color verd
                }

                else//sino
                {
                    for (int z = 0; z < letraCompr.Length; z++)//faig un altre for per poder avançar les lletres de la paraula introduida en qüestió
                    {
                        if (letraCompr[j] == palabraClave[z]) presente = true; // si la lletra esta en la paraula clau, presente es trie
                    }

                    if (presente)
                    {
                        listOfColors[i, j] = ConsoleColor.Yellow;
                        //  Print(letraCompr[j], ConsoleColor.Yellow);
                    }

                    else
                    {
                        listOfColors[i, j] = ConsoleColor.Red;
                      
                    }
                }


            }
        }

        List<string[]> lettersFiles = SelectFich(palabrasIntroducidas,list);
        PrintLetters(lettersFiles, listOfColors);

     //   PrintOthersWords()


    }

    
    List<string[]> SelectFich(List<string> palabrasIntroducidas, List<string[]> list)
    {

        string[] lettersArchivos;
        list.Clear();

        for (int i = 0; i < palabrasIntroducidas.Count; i++)
        {
            lettersArchivos = new string[5];

            for (int j = 0; j < lettersArchivos.Length; j++)
            {
                lettersArchivos[j] = File.ReadAllText(@"..\..\..\letters\" + palabrasIntroducidas[i][j] + ".txt");
            }
            list.Add(lettersArchivos);

        }

        return list; 
    }

    void Accentos()
    {
        string path = @"..\..\..\palabras.txt";
        string todo;

        using (StreamReader sr = new StreamReader(path))
        {
            todo = sr.ReadToEnd();
        }

        todo = todo.Replace('á', 'a').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('é', 'e');

        using (StreamWriter sw = new StreamWriter(@"..\..\..\palabrasSin.txt"))
        {
            sw.Write(todo);
        }

    }

    void PrintLetters(List<string[]> list, ConsoleColor[,] colors)
    {
        Console.Clear();
        Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\welcome.txt"));

        for (int i = 0; i < list.Count; i++)
        {
            
            string[] firtsLetter = list[i][0].Split("\r\n");
            string[] secondLetter = list[i][1].Split("\r\n");
            string[] thirdLetter = list[i][2].Split("\r\n");
            string[] fourdLetter = list[i][3].Split("\r\n");
            string[] fifthLetter = list[i][4].Split("\r\n");

            for (int z = 0; z < firtsLetter.GetLength(0); z++)
            {
                Console.WriteLine();

                Console.ForegroundColor = colors[i, 0];
                Console.Write(firtsLetter[z]);
                
                Console.ForegroundColor = colors[i,1];
                Console.Write(secondLetter[z]);

                Console.ForegroundColor = colors[i, 2];
                Console.Write(thirdLetter[z]);

                Console.ForegroundColor = colors[i, 3];
                Console.Write(fourdLetter[z]);

                Console.ForegroundColor = colors[i, 4];
                Console.Write(fifthLetter[z]);

                Console.ResetColor();

            }
        }



    }
}

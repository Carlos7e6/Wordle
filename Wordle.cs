/*AUTHOR: Carlos Gomez
 DATE: 12/12/2022
 DESCRIPTION: Aquesta es la meva versió del famós joc Wordle. Projecto troncal de la M03, UF1, UF2 i UF3 */

using System.Collections.Generic;
using System.Text;

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
            Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\welcome.txt"));

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
        int winIntento = 0;
        int numIntentos = 6;
        bool start = false;
        string palabra = "";//declaro l'string que utilitzare
        bool ASCII;

        palabraClave = palabraClave.ToLower();//poso a majuscules les paraules clau 


        List<string> palabrasIntroducidas = new List<string>();
        ConsoleColor[,] listOfColors = new ConsoleColor[numIntentos, 5];
        List<string[]> list = new List<string[]>();

        // string[] palabrasIntrocidas = new string[6];//les array de paraules introduides


        do//aquest for fa de contador
        {
            palabrasIntroducidas.RemoveRange(0, palabrasIntroducidas.Count);

            for (int i = 0; i <= numIntentos - 1 && palabra != "end"; i++)
            {
                if (start == true)
                {
                    Console.Clear();
                    PrintTitleWordle();
                    PrintTry(SelectFichCounter(i), i);

                    PrintOthersWords(palabrasIntroducidas, palabraClave, listOfColors, list);
                }
                else
                {
                    Console.Clear();
                    PrintTitleWordle();
                    PrintTry(SelectFichCounter(i), (numIntentos - i));
                    start = true;
                }

                do
                {
                    ASCII = false;
                    Console.WriteLine();
                    palabra = Console.ReadLine().ToLower();//demano una paraula

                    foreach (var item in palabra.ToLower())
                    {
                        if (item < 97 || item > 123) ASCII = true;
                    }
                }
                while (NotLengthWord(palabra, palabraClave) && NotTheEnd(palabra) || ASCII == true);//es fa un bucle mentres la paraulaClau no sigui igual a la paraula introduida

                palabrasIntroducidas.Add(palabra);//la palabra introducida en este count (turno) es igual a la palabra

                if (IsVictory(palabra,palabraClave))
                {
                    win = true;
                    palabra = "end";
                }
                else if (MaxTryTaked(i,numIntentos)) palabra = "end";
                winIntento = i;
            }

        } while (palabra != "end");

        VictoriaPrint(win, palabraClave, palabrasIntroducidas, listOfColors, list, winIntento);
    }


    bool NotTheEnd(string palabra)
    {
        return palabra != "end";
    }
    bool NotLengthWord(string word, string palabraClave)
    {
        return word.Length != palabraClave.Length;
    }

    bool MaxTryTaked(int i, int numIntentos)
    {
        return i == numIntentos - 1;
    }
    bool IsVictory(string palabra, string palabraClave)
    {
        return palabra == palabraClave;
    }
    void VictoriaPrint(bool win, string palabraClave, List<string> palabrasIntroducidas, ConsoleColor[,] listOfColors, List<string[]> list, int winIntento)
    {
        Console.Clear();
        string[] letras = new string[palabraClave.Length];

        PrintOthersWords(palabrasIntroducidas, palabraClave, listOfColors, list);

        for (int i = 0; i < palabraClave.Length; i++)
        {
            letras[i] = File.ReadAllText(@"..\..\..\letters\" + palabraClave[i] + ".txt");
        }


        if (win)//si las palabras son iguales
        {

            Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\win.txt"));
            PrintLettersWinOrLose(letras, ConsoleColor.Green);
            // PrintWord(palabraClave, ConsoleColor.Green);
            Console.ReadLine();
        }
        else // si les paraules son iguals i ja han pasat tots el torns
        {
            Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\lose.txt"));
            PrintLettersWinOrLose(letras, ConsoleColor.Red);
            Console.ReadLine();
        }
        Console.Clear();//clean
    }

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

                if (letraCompr[j] == palabraClave[j]) listOfColors[i, j] = ConsoleColor.Green;
                else
                {
                    for (int z = 0; z < letraCompr.Length; z++)//faig un altre for per poder avançar les lletres de la paraula introduida en qüestió
                    {
                        if (letraCompr[j] == palabraClave[z]) presente = true; // si la lletra esta en la paraula clau, presente es trie
                    }

                    if (presente) listOfColors[i, j] = ConsoleColor.Yellow;
                    else listOfColors[i, j] = ConsoleColor.Red;
                }
            }
        }

        List<string[]> lettersFiles = SelectFich(palabrasIntroducidas, list);
        PrintLetters(lettersFiles, listOfColors);
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

    void PrintLetters(List<string[]> palabras, ConsoleColor[,] colors)
    {
        //   Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\welcome.txt"));

        for (int i = 0; i < palabras.Count; i++)
        {
            string[] firtsLetter = palabras[i][0].Split("\r\n");
            string[] secondLetter = palabras[i][1].Split("\r\n");
            string[] thirdLetter = palabras[i][2].Split("\r\n");
            string[] fourdLetter = palabras[i][3].Split("\r\n");
            string[] fifthLetter = palabras[i][4].Split("\r\n");

            for (int z = 0; z < firtsLetter.GetLength(0); z++)
            {
                Console.WriteLine();

                Console.Write("\t");

                Console.ForegroundColor = colors[i, 0];
                Console.Write(firtsLetter[z]);

                Console.ForegroundColor = colors[i, 1];
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
        Console.WriteLine();

    }

    void PrintLettersWinOrLose(string[] list, ConsoleColor color)
    {
        string[] firtsLetter = list[0].Split("\r\n");
        string[] secondLetter = list[1].Split("\r\n");
        string[] thirdLetter = list[2].Split("\r\n");
        string[] fourdLetter = list[3].Split("\r\n");
        string[] fifthLetter = list[4].Split("\r\n");

        for (int z = 0; z < firtsLetter.GetLength(0); z++)
        {
            Console.WriteLine();
            Console.Write("\t");

            Console.ForegroundColor = color;
            Console.Write(firtsLetter[z]);

            Console.ForegroundColor = color;
            Console.Write(secondLetter[z]);

            Console.ForegroundColor = color;
            Console.Write(thirdLetter[z]);

            Console.ForegroundColor = color;
            Console.Write(fourdLetter[z]);

            Console.ForegroundColor = color;
            Console.Write(fifthLetter[z]);

            Console.ResetColor();
        }

    }

    string[] SelectFichCounter(int counter)
    {
        string pathAdiv = @"..\..\..\lang\es\try.txt";
        string num = @"..\..\..\nums\" + (6 - counter) + ".txt";
        string[] tryFich = new string[2];

        tryFich[0] = File.ReadAllText(pathAdiv);
        tryFich[1] = File.ReadAllText(num);

        return tryFich;
    }

    void PrintTry(string[] cabecera, int i)
    {
        string[] titulo = cabecera[0].Split("\r\n");
        string[] intento = cabecera[1].Split("\r\n");

        for (int j = 0; j < titulo.Length; j++)
        {
            Console.WriteLine();
            Console.Write(titulo[j]);

            if (i >= 5) Console.ForegroundColor = ConsoleColor.Green;
            else if (i >= 3) Console.ForegroundColor = ConsoleColor.Yellow;
            else if (i >= 1) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  " + intento[j]);
            Console.ResetColor();
        }

    }

    void PrintTitleWordle()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(File.ReadAllText(@"..\..\..\lang\es\wordle.txt"));
        Console.ResetColor();
    }
    /*  void Print(char letra, ConsoleColor color)
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
    */
}

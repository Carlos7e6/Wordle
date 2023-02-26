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
        Console.Clear();
        string select;
        string lang = @"\es\";
        string generalPath = @"..\..\..\";

        do
        {
            Console.Clear();

            Console.WriteLine(File.ReadAllText(generalPath + @"\lang\" + lang + @"welcome.txt"));
            Console.WriteLine(File.ReadAllText(generalPath + @"\lang\" + lang + @"newGame.txt"));
            Console.WriteLine(File.ReadAllText(generalPath + @"\lang\" + lang + @"selectLang.txt"));
            Console.WriteLine(File.ReadAllText(generalPath + @"\lang\" + lang + @"games.txt"));
            Console.WriteLine(File.ReadAllText(generalPath + @"\lang\" + lang + @"exit.txt"));
           

            select = Console.ReadLine();

            switch (select)
            {
                case "1":
                    Game(lang);
                    break;
                case "2":
                    lang = Languague(generalPath);
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

    string Languague(string generaPath)
    {
        string lang ="";
        string option;
        string pathLang = generaPath + @"lang\lang.txt";
        string pathTitleLang = generaPath + @"lang\titleLang.txt";

        Console.Clear();
        Console.WriteLine(File.ReadAllText(pathTitleLang));
        Console.WriteLine();
        Console.WriteLine(File.ReadAllText(pathLang));
        do
        {
            option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    lang = @"\es\";
                    break;
                case "2":
                    lang = @"\cat\";
                    break;
                default:
                    Console.WriteLine("Valor no admitido");
                    break;
            }
        }while(option != "1" && option != "2");

        return lang;
    }

    /// <summary>
    /// Esta funcion es la que engobla toda la parte del gaming
    /// </summary>
    /// <param name="lang"></param>
    void Game(string lang)
    {
        string pathLang = @"..\..\..\lang" + lang;
        string pathWords = pathLang + @"\palabrasSin.txt";
        string palabraClave;
        string palabra = "";//declaro l'string que utilitzare

    
        int winIntento = 0;
        int numIntentos = 6;

        bool win = false;
        bool ASCII;


        List<string> palabrasIntroducidas = new List<string>();
        List<string[]> listOfWords = new List<string[]>();
        ConsoleColor[,] listOfColors = new ConsoleColor[numIntentos, 5];

        do//aquest for fa de contador
        {
            palabraClave = GetWord(pathWords).ToLower();//poso a majuscules les paraules clau 
            palabrasIntroducidas.RemoveRange(0, palabrasIntroducidas.Count);

            for (int i = 0; i < numIntentos  && palabra != "end"; i++)
            {

                winIntento = i;
                if (i != 0)
                {
                    Console.Clear();
                    PrintAllThings(palabrasIntroducidas, palabraClave, listOfColors, listOfWords, pathLang, winIntento);
                }
                else
                {
                    Console.Clear();
                    PrintTitleWordle();
                    PrintTry(SelectFichCounter(winIntento,pathLang),winIntento);
                }
                do
                {
                    ASCII = false;
                    Console.WriteLine();
                    palabra = Console.ReadLine().ToLower();//demano una paraula

                    foreach (var item in palabra.ToLower())
                    {
                        if (NotGoodASCII(item)) ASCII = true;
                    }
                }
                while (NotLengthWord(palabra, palabraClave) && NotTheEnd(palabra) || ASCII == true);//es fa un bucle mentres la paraulaClau no sigui igual a la paraula introduida

                palabrasIntroducidas.Add(palabra);//la palabra introducida en este count (turno) es igual a la palabra

                if (IsVictory(palabra,palabraClave))
                {
                    win = true;
                    i = numIntentos;
                }
                else if (MaxTryTaked(i,numIntentos)) i = numIntentos;

            }
            EndPrint(win, palabraClave, palabrasIntroducidas, listOfColors, listOfWords, winIntento, pathLang);

        } while (palabra != "end");

    }
    bool NotGoodASCII(char item)
    {
        return item < 97 || item > 123;
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
    void EndPrint(bool win, string palabraClave, List<string> palabrasIntroducidas, ConsoleColor[,] listOfColors, List<string[]> list, int winIntento, string pathLang)
    {
        Console.Clear();
        string[] letras = new string[palabraClave.Length];
        ConsoleColor color;
        string winOrLose;

        for (int i = 0; i < palabraClave.Length; i++)
        {
            letras[i] = File.ReadAllText(@"..\..\..\letters\" + palabraClave[i] + ".txt");
        }

        if (win)//si las palabras son iguales
        {
            winOrLose = @"win.txt";
            color = ConsoleColor.Green;

        }
        else // si les paraules son iguals i ja han pasat tots el torns
        {
            winOrLose = @"lose.txt";
            color = ConsoleColor.Red;
        }

        PrintAllThings(palabrasIntroducidas, palabraClave, listOfColors, list, pathLang, winIntento);
        Console.WriteLine(File.ReadAllText(pathLang + winOrLose));
        PrintLettersWinOrLose(letras, color);
        Console.ReadLine();

        Console.Clear();//clean
    }
    /// <summary>
    /// Escoge una palabra aleatoria de la lista de archivos
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
    /// <summary>
    /// Printa el titulo, el numero de intentos y la lista de palabras
    /// </summary>
    /// <param name="palabrasIntroducidas"></param>
    /// <param name="palabraClave"></param>
    /// <param name="listOfColors"></param>
    /// <param name="listOfWords"></param>
    /// <param name="pathLang"></param>
    /// <param name="winIntento"></param>
    void PrintAllThings(List<string> palabrasIntroducidas, string palabraClave, ConsoleColor[,] listOfColors, List<string[]> listOfWords,string pathLang,int winIntento)
    {
        PrintTitleWordle();
        PrintTry(SelectFichCounter(winIntento, pathLang), (winIntento));
        PrintSavedWords(palabrasIntroducidas, palabraClave, listOfColors, listOfWords);
    }

    /// <summary>
    /// Printa las palabras insertadas
    /// </summary>
    /// <param name="palabrasIntroducidas"></param>
    /// <param name="palabraClave"></param>
    /// <param name="listOfColors"></param>
    /// <param name="listOfWords"></param>
    void PrintSavedWords(List<string> palabrasIntroducidas, string palabraClave, ConsoleColor[,] listOfColors, List<string[]> listOfWords)
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

        List<string[]> lettersFiles = SelectFich(palabrasIntroducidas, listOfWords);
        PrintLetters(lettersFiles, listOfColors);
    }

    /// <summary>
    /// Selecciona los ficheros de los archivos de las letras para guardarlas en una lista que despues utilizara para printar por pantalla.
    /// </summary>
    /// <param name="palabrasIntroducidas"></param>
    /// <param name="list"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Esta funcion me permite quitar los acentos de una lista de palabras
    /// </summary>
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

    /// <summary>
    /// Esta funcion printa los archivos de las letras con el color que deben tener
    /// </summary>
    /// <param name="palabras"></param>
    /// <param name="colors"></param>
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

    /// <summary>
    /// Me printa la PalabraClave, se ejecuta cuando pierda o gana la partida
    /// </summary>
    /// <param name="list"></param>
    /// <param name="color"></param>
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

    /// <summary>
    /// Selecciona los ficheros necesarios para el contador
    /// </summary>
    /// <param name="counter"></param>
    /// <param name="pathLang"></param>
    /// <returns></returns>
    string[] SelectFichCounter(int counter, string pathLang)
    {
        string pathAdiv = pathLang + @"\try.txt";
        string num = @"..\..\..\nums\" + (counter) + ".txt";
        string[] tryFich = new string[2];

        tryFich[0] = File.ReadAllText(pathAdiv);
        tryFich[1] = File.ReadAllText(num);

        return tryFich;
    }

    /// <summary>
    /// Me printa el los intentos con los ficheros que he seleccionado previamente
    /// </summary>
    /// <param name="cabecera"></param>
    /// <param name="i"></param>
    void PrintTry(string[] cabecera, int i)
    {
        string[] titulo = cabecera[0].Split("\r\n");
        string[] intento = cabecera[1].Split("\r\n");

        for (int j = 0; j < titulo.Length; j++)
        {
            Console.WriteLine();
            Console.Write(titulo[j]);

            if (i >= 6) Console.ForegroundColor = ConsoleColor.Red;
            else if (i >= 3) Console.ForegroundColor = ConsoleColor.Yellow;
            else if (i >= 1) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  " + intento[j]);
            Console.ResetColor();
        }

    }
    /// <summary>
    /// Printa el titulo de wordle
    /// </summary>
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

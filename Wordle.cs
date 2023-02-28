/*AUTHOR: Carlos Gomez
 DATE: 12/12/2022
 DESCRIPTION: Aquesta es la meva versió del famós joc Wordle. Projecto troncal de la M03, UF1, UF2 i UF3 */

using System.Text.Json;

namespace Wordlee;
public class Wordle
{
    public static void Main()
    {
        Wordle p = new Wordle();
        p.Menu();

    }

    /// <summary>
    /// Este es el menu principal del juego
    /// </summary>
    void Menu()
    {
        Console.Clear();
        string select;
        string lang = @"\es\";
        string generalPath = @"..\..\..\";
        string pathLang = generalPath + "lang" + lang;
        int counterFail = 0;

        do
        {
            Console.Clear();
            PrintAllStart(pathLang);
            if (counterFail != 0) PrintFailWord(pathLang);

            select = Console.ReadLine();

            switch (select)
            {
                case "1":
                    counterFail = 0;
                    Game(pathLang, generalPath, lang);
                    break;
                case "2":
                    counterFail = 0;
                    lang = Languague(generalPath);
                    pathLang = generalPath + "lang" + lang;
                    break;
                case "3":
                    counterFail = 0;
                    SeeHistorial(pathLang, lang);
                    break;
                case "4":
                    Console.Clear();
                    select = "end";
                    break;
                default:
                    counterFail++;
                    break;
            }

        } while (NotTheEnd(select));
    }

    /// <summary>
    /// Esta la funcion donde aparece la interfaz del idioma
    /// </summary>
    /// <param name="generaPath"></param>
    /// <returns>Retorna el lenguaje en un string</returns>
    string Languague(string generaPath)
    {
        string lang = "";
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
        } while (NotOptionValid(option));

        return lang;
    }

    /// <summary>
    /// Esta funcion es la que engobla toda la parte del gaming
    /// </summary>
    /// <param name="lang"></param>
    void Game(string pathLang, string generalPath, string lang)
    {
        string pathWords = pathLang + @"\palabrasSin.txt";
        string palabraClave;
        string palabra = "";//declaro l'string que utilitzare

        int winIntento = 0;
        int numIntentos = 6;

        bool win;
        bool ASCII;


        List<string> palabrasIntroducidas = new List<string>();
        List<string[]> listOfWords = new List<string[]>();
        ConsoleColor[,] listOfColors = new ConsoleColor[numIntentos, 5];

        do//aquest for fa de contador
        {
            win = false;
            palabraClave = GetWord(pathWords).ToLower();//poso a majuscules les paraules clau 
            palabrasIntroducidas.RemoveRange(0, palabrasIntroducidas.Count);

            for (int i = 0; i < numIntentos && palabra != "end"; i++)
            {
                int counterFailWord = 0;
                winIntento = i;
                if (i != 0)//Aqui lo unico que no printo son las letras introducidas porque aun no hay
                {
                    Console.Clear();
                    PrintAllThings(palabrasIntroducidas, palabraClave, listOfColors, listOfWords, pathLang, winIntento, generalPath);
                }
                else
                {
                    Console.Clear();
                    PrintTitleWordle(pathLang);
                    PrintTry(SelectFichCounter(winIntento, pathLang, generalPath), winIntento);
                }
                do
                {
                    if (counterFailWord != 0) PrintFailWord(pathLang);

                    if (palabra.Contains("ç") && lang == @"\cat\") Console.WriteLine("El caràcter 'Ç' no està soportado");
                    else if (palabra.Contains("ç") && lang == @"\es\") Console.WriteLine("El carácter 'Ç' no está soportado");

                    if (palabra.Contains("ñ") && lang == @"\cat\") Console.WriteLine("El caràcter 'Ñ' no està soportat");
                    else if (palabra.Contains("ñ") && lang == @"\es\") Console.WriteLine("El carácter 'Ñ' no está soportado");

                    ASCII = false;
                    Console.WriteLine();
                    palabra = Console.ReadLine().ToLower();//demano una paraula

                    foreach (var item in palabra.ToLower())
                    {
                        if (NotGoodASCII(item)) ASCII = true;
                    }
                    counterFailWord++;
                }
                while (NotLengthWord(palabra, palabraClave) && NotTheEnd(palabra) || ASCII == true);//es fa un bucle mentres la paraulaClau no sigui igual a la paraula introduida

                palabrasIntroducidas.Add(palabra);//la palabra introducida en este count (turno) es igual a la palabra

                if (IsVictory(palabra, palabraClave))//comprueba si se ha ganado
                {
                    win = true;
                    i = numIntentos;
                }
                else if (MaxTryTaked(i, numIntentos)) i = numIntentos;

            }
            if (palabra != "end") EndPrint(win, palabraClave, palabrasIntroducidas, listOfColors, listOfWords, winIntento, pathLang, generalPath, lang);
            palabra = RetryGame(pathLang);
        } while (palabra != "end");

    }
    /// <summary>
    /// La funcion devuelve TRUE cuando las palabras no estan dentro del codigo ASCII deseado
    /// </summary>
    /// <param name="item"></param>
    /// <returns>bool</returns>
    public static bool NotGoodASCII(char item)
    {
        return item < 97 || item > 123;
    }


    /// <summary>
    /// Devuelve TRUE cuando la opcion escogida NO es valida
    /// </summary>
    /// <param name="option"></param>
    /// <returns>bool</returns>
    public static bool NotOptionValid(string option)
    {
        return option != "1" && option != "2";
    }

    /// <summary>
    /// Retorna TRUE si la palabra recibida no es "end"
    /// </summary>
    /// <param name="palabra"></param>
    /// <returns>bool</returns>
    public static bool NotTheEnd(string palabra)
    {
        return palabra != "end";
    }

    /// <summary>
    /// Devuelve TRUE si el Lenght de la palabra no es el adecuado
    /// </summary>
    /// <param name="word"></param>
    /// <param name="palabraClave"></param>
    /// <returns>bool</returns>
    public static bool NotLengthWord(string word, string palabraClave)
    {
        return word.Length != palabraClave.Length;
    }

    /// <summary>
    /// Devuelve TRUE si el numero maximo de intentos es el mismo que los que lleva
    /// </summary>
    /// <param name="i"></param>
    /// <param name="numIntentos"></param>
    /// <returns>bool</returns>
    public static bool MaxTryTaked(int i, int numIntentos)
    {
        return i >= numIntentos - 1;
    }

    /// <summary>
    /// Si las palabras coinciden devuelve TRUE
    /// </summary>
    /// <param name="palabra"></param>
    /// <param name="palabraClave"></param>
    /// <returns>bool</returns>
    public static bool IsVictory(string palabra, string palabraClave)
    {
        return palabra == palabraClave;
    }

    /// <summary>
    /// Devuelve "end" si no se quiere continuar el juego y "" si sí.
    /// </summary>
    /// <param name="pathLang"></param>
    /// <returns></returns>
    public static string RetryGame(string pathLang)
    {
        string retry = "";
        string option;

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(File.ReadAllText(pathLang + @"retry.txt"));
        Console.WriteLine(File.ReadAllText(pathLang + @"yn.txt"));

        do
        {
            option = Console.ReadLine().ToLower();
            switch (option)
            {
                case "y":
                    retry = "";
                    break;
                case "n":
                    retry = "end";
                    break;
                default:
                    PrintFailWord(pathLang);
                    break;
            }
        } while (NotValidAnswerRetry(option));
        return retry;


    }

    /// <summary>
    /// Retorna TRUE cuando las opciones NO son validas
    /// </summary>
    /// <param name="option"></param>
    /// <returns>bool</returns>
    public static bool NotValidAnswerRetry(string option)
    {
        return option != "y" && option != "n";
    }

    /// <summary>
    /// Este procedimiento escribe en el documento historial
    /// </summary>
    /// <param name="pathLang"></param>
    /// <param name="intentos"></param>
    /// <param name="palabrasIntr"></param>
    /// <param name="palabraClave"></param>
    void AddGameToFile(string pathLang, int intentos, List<string> palabrasIntr, string palabraClave, ConsoleColor color, string dateOf, string lang)
    {
        Console.WriteLine(File.ReadAllText(pathLang + @"\yourName.txt"));
        string player = Console.ReadLine();

        var gamePlayer = CreatePlayer(player, lang.ToUpper(), palabraClave, palabrasIntr, intentos, color, dateOf);

        using StreamWriter sw = File.AppendText(pathLang + @"\historial.txt");
        {
            sw.WriteLine(JsonSerializer.Serialize(gamePlayer));
        }
    }

    /// <summary>
    /// Utilizamos este procedimiento para ver el historial
    /// </summary>
    /// <param name="pathLang"></param>
    void SeeHistorial(string pathLang, string lang)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine(File.ReadAllText(pathLang + @"games.txt"));

        using (StreamReader sr = new StreamReader(pathLang + @"\historial.txt"))
        {
            string lane;
            while ((lane = sr.ReadLine()) != null)
            {
                var game = JsonSerializer.Deserialize<PlayerGame>(lane);
                Console.WriteLine();
                if (lang == @"\es\")
                {
                    Console.Write($"\tPlayer: {game.playerName} \tFecha: {game.date} \tIntentos: {game.contador} \tPalabraSecreta: ");
                    Console.ForegroundColor = game.color;
                    Console.Write(game.secretWord);
                    Console.ResetColor();
                    Console.Write("\tPalabras: ");
                }
                else
                {
                    Console.Write($"\tPlayer: {game.playerName} \tData: {game.date} \tIntents: {game.contador} \tParaulaSecreta: ");
                    Console.ForegroundColor = game.color;
                    Console.Write(game.secretWord);
                    Console.ResetColor();
                    Console.Write("\tParaules: ");
                }
                foreach (var item in game.listWord)
                {
                    Console.Write(" | " + item);
                }
                Console.WriteLine();
                Console.WriteLine("\n\n");

            }

            if (lang == @"\es\") Console.WriteLine("[PULSA CUALQUIER INPUT PARA SALIR]");
            else Console.WriteLine("[PREM QUALSEVOL INPUT PER SORTIR]");
            Console.ReadLine();

        }
    }

    /// <summary>
    /// Este procedimiento hace el print final te todos los datos del juego necesarios
    /// </summary>
    /// <param name="win"></param>
    /// <param name="palabraClave"></param>
    /// <param name="palabrasIntroducidas"></param>
    /// <param name="listOfColors"></param>
    /// <param name="list"></param>
    /// <param name="winIntento"></param>
    /// <param name="pathLang"></param>
    /// <param name="generalPath"></param>
    void EndPrint(bool win, string palabraClave, List<string> palabrasIntroducidas, ConsoleColor[,] listOfColors, List<string[]> list, int winIntento, string pathLang, string generalPath, string lang)
    {
        Console.Clear();
        string[] letras = new string[palabraClave.Length];
        ConsoleColor color;
        string winOrLose;
        winIntento++;

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

        PrintAllThings(palabrasIntroducidas, palabraClave, listOfColors, list, pathLang, winIntento, generalPath);
        Console.WriteLine(File.ReadAllText(pathLang + winOrLose));
        PrintLettersWinOrLose(letras, color);
        Console.ReadLine();

        AddGameToFile(pathLang, winIntento, palabrasIntroducidas, palabraClave, color, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), lang);

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
    void PrintAllThings(List<string> palabrasIntroducidas, string palabraClave, ConsoleColor[,] listOfColors, List<string[]> listOfWords, string pathLang, int winIntento, string generalPath)
    {
        PrintTitleWordle(pathLang);
        PrintTry(SelectFichCounter(winIntento, pathLang, generalPath), (winIntento));
        PrintSavedWords(palabrasIntroducidas, palabraClave, listOfColors, listOfWords, generalPath);
    }

    /// <summary>
    /// Printa las palabras insertadas
    /// </summary>
    /// <param name="palabrasIntroducidas"></param>
    /// <param name="palabraClave"></param>
    /// <param name="listOfColors"></param>
    /// <param name="listOfWords"></param>
    void PrintSavedWords(List<string> palabrasIntroducidas, string palabraClave, ConsoleColor[,] listOfColors, List<string[]> listOfWords, string generalPath)
    {
        bool presente;

        for (int i = 0; i < palabrasIntroducidas.Count; i++)//aixo ho utilitzare per printar totes les paraules que mha donat l'usuari, per aixo el delimitador del for es el count dels torns
        {
            Console.WriteLine();
            string palabraCompr = palabrasIntroducidas[i];//escolleixo de l'array de paraules introduides
            for (int j = 0; j < palabraClave.Length; j++)//aquest for l'utilitzare per comparar una posicio en concret de la paraula introduida
            {
                presente = false;//igualo presente a false

                if (palabraCompr[j] == palabraClave[j]) listOfColors[i, j] = ConsoleColor.Green;
                else
                {

                    for (int z = 0; z < palabraCompr.Length; z++)
                    {
                        if ((palabraCompr[j] == palabraClave[z]) && (palabraCompr[z] != palabraClave[z])) //printara amarrillo si esta dentro de la palabra y si la letra no esta acertada 
                        {
                            presente = true;
                        }
                        // si la lletra esta en la paraula clau, presente es trie
                    }
                    if (presente)
                    {
                        listOfColors[i, j] = ConsoleColor.Yellow;
                    }
                    else listOfColors[i, j] = ConsoleColor.Red;
                }
            }
        }

        List<string[]> lettersFiles = SelectFich(palabrasIntroducidas, listOfWords, generalPath);
        PrintLetters(lettersFiles, listOfColors);
    }

    /// <summary>
    /// Selecciona los ficheros de los archivos de las letras para guardarlas en una lista que despues utilizara para printar por pantalla.
    /// Esta funcion no tiene un UnitTest por la gran cantidad de strings que contiene
    /// </summary>
    /// <param name="palabrasIntroducidas"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    List<string[]> SelectFich(List<string> palabrasIntroducidas, List<string[]> list, string generalPath)
    {

        string[] lettersArchivos;
        list.Clear();

        for (int i = 0; i < palabrasIntroducidas.Count; i++)
        {
            lettersArchivos = new string[5];

            for (int j = 0; j < lettersArchivos.Length; j++)
            {
                lettersArchivos[j] = File.ReadAllText(generalPath + @"\letters\" + palabrasIntroducidas[i][j] + ".txt");
            }
            list.Add(lettersArchivos);

        }

        return list;
    }

    /// <summary>
    /// Esta funcion printa los archivos de las letras con el color que deben tener
    /// </summary>
    /// <param name="palabras"></param>
    /// <param name="colors"></param>
    void PrintLetters(List<string[]> palabras, ConsoleColor[,] colors)
    {
        for (int i = 0; i < palabras.Count; i++)//aqui asigno un string de la letra(imagen) a cada letra segun la posicion de la lista
        {
            string[] firtsLetter = palabras[i][0].Split("\r\n");
            string[] secondLetter = palabras[i][1].Split("\r\n");
            string[] thirdLetter = palabras[i][2].Split("\r\n");
            string[] fourdLetter = palabras[i][3].Split("\r\n");
            string[] fifthLetter = palabras[i][4].Split("\r\n");

            for (int z = 0; z < firtsLetter.GetLength(0); z++)//aqui printo de arriba a abajo todas las letras simultaneamente con el color deseado
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
    void PrintLettersWinOrLose(string[] list, ConsoleColor color) //Printo las letras pero esta vez solo me llega una palabra (string[]) con las respectivas imagenes de sus letras
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
    public static string[] SelectFichCounter(int counter, string pathLang, string generalPath)
    {
        string pathAdiv = pathLang + @"\try.txt";
        string num = generalPath + @"\nums\" + (counter) + ".txt";
        string[] tryFich = new string[2];

        tryFich[0] = File.ReadAllText(pathAdiv);
        tryFich[1] = File.ReadAllText(num);

        return tryFich;
    }

    /// <summary>
    /// Este procedimiento printa los intentos con los ficheros que he seleccionado previamente
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

            if (i > 3) Console.ForegroundColor = ConsoleColor.Red;
            else if (i > 1) Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.Green;

            Console.Write("  " + intento[j]);
            Console.ResetColor();
        }

    }

    /// <summary>
    /// Printa el titulo de wordle
    /// </summary>
    void PrintTitleWordle(string pathLang)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(File.ReadAllText(pathLang + @"\wordle.txt"));
        Console.ResetColor();
    }

    /// <summary>
    /// Printa la interfaz del menu
    /// </summary>
    /// <param name="pathLang"></param>
    void PrintAllStart(string pathLang)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(File.ReadAllText(pathLang + @"welcome.txt"));
        Console.ResetColor();
        Console.WriteLine(File.ReadAllText(pathLang + @"newGame.txt"));
        Console.WriteLine(File.ReadAllText(pathLang + @"selectLang.txt"));
        Console.WriteLine(File.ReadAllText(pathLang + @"games.txt"));
        Console.WriteLine(File.ReadAllText(pathLang + @"exit.txt"));
    }

    /// <summary>
    /// Printa una advertencia de que el input utilizado no es el adecuado
    /// </summary>
    /// <param name="pathLang"></param>
    static void PrintFailWord(string pathLang)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(File.ReadAllText(pathLang + @"fail.txt"));
        Console.ResetColor();
    }

    /// <summary>
    /// Esta funcion me retorna el objeto PlayerGame
    /// </summary>
    /// <param name="playerNameOf"></param>
    /// <param name="gameLanguageOf"></param>
    /// <param name="secretWordOf"></param>
    /// <param name="listWordOf"></param>
    /// <param name="contadorOf"></param>
    /// <param name="colorOf"></param>
    /// <param name="dateOf"></param>
    /// <returns>Object PlayerGame</returns>
    public static PlayerGame CreatePlayer(string playerNameOf, string gameLanguageOf, string secretWordOf, List<string> listWordOf, int contadorOf, ConsoleColor colorOf, string dateOf)
    {
        PlayerGame jugador = new PlayerGame
        {
            playerName = playerNameOf,
            date = dateOf,
            gameLanguage = gameLanguageOf,
            secretWord = secretWordOf,
            listWord = listWordOf,
            contador = contadorOf,
            color = colorOf
        };

        return jugador;
    }
}

//He creado esta clase para crear partidas para el historial max facilmente
public class PlayerGame
{
    public string playerName { get; set; }
    public string date { get; set; }
    public string gameLanguage { get; set; }
    public string secretWord { get; set; }
    public List<string> listWord { get; set; }
    public int contador { get; set; }
    public ConsoleColor color { get; set; }

}

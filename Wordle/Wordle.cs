/*AUTHOR: Carlos Gomez
 DATE: 12/12/2022
 DESCRIPTION: Aquesta es la meva versió del famós joc Wordle. Projecto troncal de la M03, UF1, UF2 i UF3 */
public class Wordle
{
    public static void Main()
    {
        string[] palabras = { "gatos", "adios", "queso", "canto", "oreas", "cinto", "listo", "adose", "Busto", "Burla", "Bueno", "Lunar", "Novel", "Surco", "Solar", "Zanco", "Exudo", "Caliz", "Debil", "Dogma" };//llista de paraules clau
        Random random = new Random();// un numero aleatori per seleccionar
        int randomNum = random.Next(palabras.Length);

        string palabraClave = palabras[randomNum];
        palabraClave = palabraClave.ToUpper();//poso a majuscules les paraules clau 
        string palabra;//declaro l'string que utilitzare

        bool presente;//un bool que sera per saber si una lletra está dintre de la paraula o no
        string[] palabrasIntrocidas = new string[6];//les array de paraules introduides

        
        for (int count = 0; count < 6; count++)//aquest for fa de contador
        {
            Console.WriteLine("Wordle");//el titol del joc
            Console.WriteLine("Endevina la paraula");
         ///////////////////////////////////////////////////////////////////
            for (int i = 0; i < count; i++)//aixo ho utilitzare per printar totes les paraules que mha donat l'usuari, per aixo el delimitador del for es el count dels torns
            {
                Console.WriteLine();
                string letraCompr = palabrasIntrocidas[i];//escolleixo de l'array de paraules introduides
                for (int j = 0; j < palabraClave.Length; j++)//aquest for l'utilitzare per comparar una posicio en concret de la paraula introduida
                {
                    presente = false;//igualo presente a false
                    if (count != 0)// si el numero de count (rondes) no es 0
                    {
                        if (letraCompr[j] == palabraClave[j])//si les dos lletres son iguals
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(letraCompr[j] + "\t"); //printo la lletra de color verd
                            Console.ResetColor();
                        }
                        else//sino
                        {
                            for (int z = 0; z < letraCompr.Length; z++)//faig un altre for per poder avançar les lletres de la paraula introduida en qüestió
                            {
                                if (letraCompr[j] == palabraClave[z]) presente = true; // si la lletra esta en la paraula clau, presente es trie
                            }
                            if (presente)//if presente
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(letraCompr[j] + "\t"); //printo la lletra de color groc
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;//printo la lletra de color vermell 
                                Console.Write(letraCompr[j] + "\t");
                                Console.ResetColor();
                            }
                        }
                    }

                }
         //////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Tens " + (6 - count) + " intents");//printo quants intents tinc encara

            do
            {
                Console.WriteLine();
                Console.WriteLine("Donam una paraula amb 5 lletres \n ");
                palabra = Console.ReadLine();//demano una paraula
            }
            while (palabraClave.Length != palabra.Length);//es fa un bucle mentres la paraulaClau no sigui igual a la paraula introduida


            palabra = palabra.ToUpper();//poso en majuscula les paraules
            palabrasIntrocidas[count] = palabra;//la palabra introducida en este count (turno) es igual a la palabra

            if (palabra == palabraClave)//si las palabras son iguales
            {
                count = 6;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(palabra);
                Console.WriteLine("¡Felicidades, has ganado!");
                Console.ResetColor(); //printo el final si has guanyat
                Console.ReadLine();
            }
            else if (palabra != palabraClave & count == 5) // si les paraules son iguals i ja han pasat tots el torns
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(palabraClave + "\n ");//printo el final si has perdut
                Console.WriteLine("¡Has perdido!");
                Console.ReadLine();
            }
            Console.Clear();//clean
            
        }

    }
}
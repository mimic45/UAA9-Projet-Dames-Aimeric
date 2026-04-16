using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Dames
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int tour = 2;


            int[,] plateau = new int[8, 8];
            string pionNoir = "o";
            string pionBlanc = "O";

            InitialiserPartie(plateau);

            Console.WriteLine("Bienvenue dans ce jeu de Dames");
            Console.WriteLine("===============================");
            Console.WriteLine("Quel niveau doit etre le bot ?");
            int niveau = int.Parse(Console.ReadLine());


            int nbNoirs = 0;
            int nbBlanc = 0;
            do
            {
                // si c est le tour du joueur (2)
                if (tour == 2)
                {
                    Console.WriteLine("C'EST AUX BLANCS (O)");
                    CreaPlat(plateau, pionNoir, pionBlanc);

                    bool obligationDeManger = PeutManger(plateau, tour);

                    if (obligationDeManger)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("!!! VOUS AVEZ UNE PRISE OBLIGATOIRE !!!");
                        Console.ResetColor();
                    }

                    bool coupReussi = Deplacement(plateau, 0, 0, tour, obligationDeManger);
                    if (coupReussi)
                    {
                        tour = 1;
                    }
                    
                }
                else if (tour == 1)
                {
                    Console.WriteLine("C'EST AUX NOIRS (o)");
                    TourDuBot(plateau, niveau);
                    CreaPlat(plateau, pionNoir, pionBlanc);
                    Console.ReadKey();
                    tour = 2;
                }
                

                nbNoirs = 0;
                nbBlanc = 0;


                for (int iLigne = 0; iLigne < 8; iLigne++)
                {
                    for (int iCol = 0; iCol < 8; iCol++)
                    {
                        if (plateau[iLigne, iCol] == 1 || plateau[iLigne, iCol] == 4)
                        {
                            nbNoirs++;
                        }
                        else if (plateau[iLigne, iCol] == 2 || plateau[iLigne, iCol] == 3)
                        {
                            nbBlanc++;
                        }

                    }
                }

            } while (nbNoirs > 0 && nbBlanc > 0);
            Console.Clear();
            if (nbNoirs == 0)
            {
                Console.WriteLine("Les BLANCS ont gagné");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Les NOIRS ont gagné");
                Console.ReadKey();
            }
        }

        static bool EstDiagonale(int l1, int c1, int l2, int c2)
        {
            int distL = Math.Abs(l2 - l1);
            int distC = Math.Abs(c2 - c1);

            // On renvoie vrai si les distances sont égales (mouvement diagonal)
            return distL == distC;
            /*if (distL == distC)
               
                return true;
            }
            else
            {
                return false;
            } c'est la même chose */
        }

        static int AnalyserChemin(int[,] plateau, int l1, int c1, int l2, int c2, int tour)
        {
            int dirL = (l2 > l1) ? 1 : -1;
            int dirC = (c2 > c1) ? 1 : -1;

            int nbEnnemis = 0;
            int ligneTest = l1 + dirL;
            int colTest = c1 + dirC;

            // Boucle qui "marche" sur la diagonale jusqu'à la case juste avant l'arrivée
            while (ligneTest != l2 && colTest != c2)
            {
                int piece = plateau[ligneTest, colTest];

                if (piece != 0) // Si la case n'est pas vide
                {
                    // regarde si c est un allier
                    bool estAllie = (tour == 2 && (piece == 2 || piece == 3)) ||
                                   (tour == 1 && (piece == 1 || piece == 4));

                    if (estAllie) return 0; // bloqué par un ami

                    // Sinon c'est un ennemi
                    nbEnnemis++;
                }

                if (nbEnnemis > 1) return 0; 

                ligneTest += dirL;
                colTest += dirC;
            }

            if (nbEnnemis == 1) return 2;
            return 1; 
        }

        // placement des pions
        static void InitialiserPartie(int[,] plateau)
        {
            for (int iLigne = 0; iLigne < 8; iLigne++)
            {
                for (int iCol = 0; iCol < 8; iCol++)
                {
                    if ((iLigne + iCol) % 2 == 0)
                    {
                        if (iLigne == 0 || iLigne == 1)
                        {
                            plateau[iLigne, iCol] = 1;
                        }
                        else if (iLigne == 6 || iLigne == 7)
                        {
                            plateau[iLigne, iCol] = 2;
                        }
                    }

                }
            }
        }

        static void CreaPlat(int[,] plateau, string pionNoir, string pionBlanc)
        {
            Console.WriteLine("\n      0   1   2   3   4   5   6   7");

            Console.Write("    ╔");
            for (int i = 0; i < 7; i++) Console.Write("═══╦");
            Console.WriteLine("═══╗");

            for (int iLigne = 0; iLigne < 8; iLigne++)
            {
                Console.Write(" " + iLigne + "  ║");

                for (int iCol = 0; iCol < 8; iCol++)
                {
                    int piece = plateau[iLigne, iCol];

                    if ((iLigne + iCol) % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write(" ");

                    switch (piece)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(pionNoir);
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(pionBlanc);
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("D");
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("D");
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }

                    Console.Write(" ");
                    Console.ResetColor();
                    Console.Write("║");
                }

                Console.WriteLine();

                if (iLigne < 7)
                {
                    Console.Write("    ╠");
                    for (int i = 0; i < 7; i++) Console.Write("═══╬");
                    Console.WriteLine("═══╣");
                }
            }

            Console.Write("    ╚");
            for (int i = 0; i < 7; i++) Console.Write("═══╩");
            Console.WriteLine("═══╝\n");
        }

        static bool Deplacement(int[,] plateau, int ligne, int col, int tour, bool obligationDeManger)
        {
            Console.WriteLine("Ligne du pion à bouger (0-7) :");
            ligne = int.Parse(Console.ReadLine());
            Console.WriteLine("Colonne du pion à bouger (0-7) :");
            col = int.Parse(Console.ReadLine());

            int pion = plateau[ligne, col];

            
            if (plateau[ligne, col] == 0 || (tour == 2 && plateau[ligne, col] != 2 && plateau[ligne, col] != 3) || (tour == 1 && plateau[ligne, col] != 1 && plateau[ligne, col] != 4))
            {
                Console.WriteLine("Ce n'est pas votre pion !");
                Console.ReadLine();
                return false;
            }

            Console.WriteLine("Ligne d'arrivée :");
            int ligneArrive = int.Parse(Console.ReadLine());
            Console.WriteLine("Colonne d'arrivée :");
            int colArrivee = int.Parse(Console.ReadLine());

            
            if (EstDiagonale(ligne, col, ligneArrive, colArrivee) == false)
            {
                Console.WriteLine("Mouvement non diagonal interdit !");
                Console.ReadLine();
                return false;
            }

            int distanceLigne = Math.Abs(ligneArrive - ligne);

            if (obligationDeManger == true && distanceLigne != 2)
            {
                Console.WriteLine("Pas possible!!");
                return false;
            }

            // 3. Règles spécifiques aux pions (pas les Dames)
            if ((pion == 1 || pion == 2) && distanceLigne > 2)
            {
                Console.WriteLine("Un pion ne peut pas sauter aussi loin !");
                Console.ReadLine();
                return false;
            }

            if (pion == 2 && ligneArrive > ligne) // Le pion blanc ne recule pas
            {
                Console.WriteLine("Un pion ne peut pas reculer !");
                Console.ReadLine();
                return false;
            }

            if (pion == 1 && ligneArrive < ligne) // Le pion noir (bot) ne recule pas
            {
                Console.WriteLine("Un pion ne peut pas reculer !");
                Console.ReadLine();
                return false;
            }

            // 4. Case d'arrivée vide ?
            if (plateau[ligneArrive, colArrivee] != 0)
            {
                Console.WriteLine("Case occupée !");
                Console.ReadLine();
                return false;
            }

            // 5. Analyse du chemin (Obstacles et prises)
            int resultatChemin = AnalyserChemin(plateau, ligne, col, ligneArrive, colArrivee, tour);

            if (resultatChemin == 0)
            {
                Console.WriteLine("Mouvement impossible (chemin bloqué) !");
                Console.ReadLine();
                return false;
            }

            
            if (resultatChemin == 2)
            {
                int dirL = (ligneArrive > ligne) ? 1 : -1;
                int dirC = (colArrivee > col) ? 1 : -1;
                int tempL = ligne + dirL;
                int tempC = col + dirC;

                while (plateau[tempL, tempC] == 0)
                {
                    tempL += dirL;
                    tempC += dirC;
                }
                plateau[tempL, tempC] = 0; // Suppression du pion mangé
                Console.WriteLine("Pion mangé !");
            }

            plateau[ligne, col] = 0; // Ancienne case vide
            plateau[ligneArrive, colArrivee] = pion; // Nouveau placement

            // Transformation en Dame
            if (ligneArrive == 0 && pion == 2)
            {
                plateau[ligneArrive, colArrivee] = 3;
            }
            else if (ligneArrive == 7 && pion == 1) 
            {
                plateau[ligneArrive, colArrivee] = 4;
            }
            return true;
        }
     

        static void TourDuBot(int[,] plateau, int niveau)
        {
            if (niveau == 1)
            {
                Random alea = new Random();
                bool aJoue = false;
                while (!aJoue)
                {
                    int iLigne = alea.Next(0, 8);
                    int iCol = alea.Next(0, 8);

                    if (plateau[iLigne, iCol] == 1)
                    {
                        int departL = iLigne;
                        int departCol = iCol;

                        int arriveL = iLigne + 1;
                        int arriveColDroite = iCol + 1;
                        int arriveColGauche = iCol - 1;

                        if (arriveL < 8 && arriveColDroite < 8 && plateau[arriveL, arriveColDroite] == 0)
                        {
                            plateau[arriveL, arriveColDroite] = 1;
                            plateau[departL, departCol] = 0;
                            aJoue = true;
                        }
                        else if (arriveL < 8 && arriveColGauche >= 0 && plateau[arriveL, arriveColGauche] == 0)
                        {
                            plateau[arriveL, arriveColGauche] = 1;
                            plateau[departL, departCol] = 0;
                            aJoue = true;
                        }
                    }
                }
            }
            else if (niveau == 2)
            {
                for (int iLigne = 0; iLigne < 8; iLigne++)
                {
                    for (int iCol = 0; iCol < 8; ++iCol)
                    {
                        if (plateau[iLigne, iCol] == 1)
                        {
                            if (iLigne + 2 < 8 && iCol + 2 < 8)
                            {
                                if (plateau[iLigne + 1, iCol + 1] == 2 && plateau[iLigne + 2, iCol + 2] == 0)
                                {
                                    plateau[iLigne, iCol] = 0;
                                    plateau[iLigne + 1, iCol + 1] = 0;
                                    plateau[iLigne + 2, iCol + 2] = 1;
                                    return;
                                }
                            }

                            if (iLigne + 2 < 8 && iCol - 2 >= 0)
                            {
                                if (plateau[iLigne + 1, iCol - 1] == 2 && plateau[iLigne + 2, iCol - 2] == 0)
                                {
                                    plateau[iLigne, iCol] = 0;
                                    plateau[iLigne + 1, iCol - 1] = 0;
                                    plateau[iLigne + 2, iCol - 2] = 1;
                                    return;
                                }
                            }
                        }
                    }
                }


                Random alea = new Random();

                bool aJoue = false;
                int essais = 0;
                while (!aJoue && essais < 200)
                {
                    int iLigne = alea.Next(0, 8);
                    int iCol = alea.Next(0, 8);

                    if (plateau[iLigne, iCol] == 1)
                    {
                        int departL = iLigne;
                        int departCol = iCol;

                        int arriveL = iLigne + 1;

                        int direction = alea.Next(0, 2) * 2 - 1;

                        int arriveCol = iCol + direction;

                        if (arriveL < 8 && arriveCol < 8 && arriveCol >= 0 && plateau[arriveL, arriveCol] == 0)
                        {
                            plateau[arriveL, arriveCol] = 1;
                            plateau[departL, departCol] = 0;
                            aJoue = true;
                        }
                    }
                    essais++;
                }
            }
        }

        static bool PeutManger(int[,] plateau, int tour)
        {
            int[] dL = { 1, 1, -1, -1 };
            int[] dC = { 1, -1, 1, -1 };

            for (int iLigne = 0; iLigne < 8; iLigne++)
            {
                for (int iCol = 0; iCol < 8; iCol++)
                {
                    int piece = plateau[iLigne, iCol];

                    if ((tour == 2 && (piece == 2 || piece == 3)) || (tour == 1 && (piece == 1 || piece == 4)))
                    {
                        for (int direction = 0; direction < 4; direction++)
                        {
                            int lignArr = iLigne + (dL[direction] * 2);
                            int colArr = iCol + (dC[direction] * 2);

                           
                            if (lignArr >= 0 && lignArr < 8 && colArr >= 0 && colArr < 8)
                            {
                                
                                if (plateau[lignArr, colArr] == 0)
                                {
                                    
                                    if (AnalyserChemin(plateau, iLigne, iCol, lignArr, colArr, tour) == 2)
                                    {
                                        return true; 
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false; 
        }
    }
}
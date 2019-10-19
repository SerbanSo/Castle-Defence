using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

//FISIER : Suprafata Suprafata NumarulPatratelorDrum NumarTurnuri DimensiunePatrat
//####TO DO LIST####
// Refacut baraHP inamici
// Bug miscare



namespace Atestat
{
    public partial class Suprafata : Form
    {
        //Matricea de joc
        int[,] matriceSuprafata;

        //Pozitia inamicului in structura DrumInamici
        List<int> nrPozitieInamic = new List<int>();

        //Dimensiunea unui patrat
        int dimensiunePatrat;

        //Numarul de turnuri si de patrate din care e alcatuit drumul
        int nrTurnuri, nrPatrateDrum;

        //Numar de linii/coloane
        int nrLinii;

        //Inamici
        List<Inamici> inamic = new List<Inamici>();
        int nrInamici = 10;

        //Turnuri + nr curent de turnuri
        TurnTier1[] turn;
        int nrTurn;

        //pozitia pe axa Ox si Oy a drumului inamicilor + pozitia acestora la un moment de timp
        struct DrumInamici
        {
            int _x;
            public int x
            {
                set
                {
                    _x = value;
                }
                get
                {
                    return _x;
                }
            }
            int _y;
            public int y
            {
                set
                {
                    _y = value;
                }
                get
                {
                    return _y;
                }
            }
            static int _lungime;
            public static  int lungime
            {
                set
                {
                    _lungime = value;
                }
                get
                {
                    return _lungime;
                }
            }
        };
        DrumInamici[] drumInamici;

        //pozitie pe axa Ox si Oy a turnurilor
        struct PozitieTunuri
        {
            int _x;
            public int x
            {
                set
                {
                    _x = value;
                }
                get
                {
                    return _x;
                }
            }
            int _y;
            public int y
            {
                set
                {
                    _y = value;
                }
                get
                {
                    return _y;
                }
            }
        }
        PozitieTunuri[] pozitieTurnuri;

        //Bara hp inamici    
        List<ProgressBar> baraHP = new List<ProgressBar>();
        List<Label> numeHPInamic = new List<Label>();

        //score
        int score = 0;

        public Suprafata()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader("Map.txt");
            Initializare_Variabile_Start(sr);
            Initializare_Suprafata_Joc(sr);
            Initializare_Drum_Inamici();
            Initializare_Inamici();
            Initializare_Pozitie_Tunuri();
            Initializare_HP_Inamici();

            //Schimbare parinte la picturebox2 pentru a avea imaginea transparenta
            pictureBox2.Parent = pictureBox1;
            pictureBox2.Visible = false;

            turn = new TurnTier1[nrTurnuri];

            timer1.Enabled = true;
            timer2.Enabled = true;
        }

        private void Initializare_Variabile_Start(StreamReader sr)
        {
            string _s = sr.ReadLine();
            string[] _sir = _s.Split(' ');
            matriceSuprafata = new int[int.Parse(_sir[0]), int.Parse(_sir[1])];
            nrPatrateDrum = int.Parse(_sir[2]);
            nrTurnuri = int.Parse(_sir[3]);
            dimensiunePatrat = int.Parse(_sir[4]);
            nrLinii = int.Parse(_sir[0]);
        }

        private void Initializare_Suprafata_Joc(StreamReader sr)
        {
            // Initializare suprafata de joc
            string _s;
            string[] _sir = new string[dimensiunePatrat];
            for (int i = 0; i < nrLinii; i++)
            {
                _s = sr.ReadLine();
                _sir = _s.Split(' ');
                for (int j = 0; j < nrLinii; j++)
                {
                    matriceSuprafata[i, j] = int.Parse(_sir[j]);
                }
            }
        }

        private void Initializare_Drum_Inamici()
        {
            // Initializare struct de drum-inamici
            drumInamici = new DrumInamici[nrPatrateDrum];
            int z = 0;
            for (int i = 0; i < nrLinii; i++)
            {
                for (int j = 0; j < nrLinii; j++)
                {
                    if (matriceSuprafata[i, j] == 1 || matriceSuprafata[i, j] == -1)
                    {
                        drumInamici[z].x = j;
                        drumInamici[z].y = i;
                        z++;
                    }
                }
            }
            DrumInamici.lungime = z-1;
        }        

        private void Initializare_Inamici()
        {
            //rx si ry pun o pozitie random in casuta principala
            Random r = new Random();
            for (int i = 0; i < nrInamici; i++)
            {
                int x = r.Next(0, dimensiunePatrat - 10);
                int y = r.Next(0, dimensiunePatrat - 10);
                inamic.Add(new Inamici(drumInamici[0].x + x, drumInamici[0].y + y, 10, 100, x, y, r.Next(1, 4)));
                nrPozitieInamic.Add(1);
            }
        }

        private void Initializare_Pozitie_Tunuri()
        {
            pozitieTurnuri = new PozitieTunuri[nrTurnuri];
            int z = 0;
            for (int i = 0; i < nrLinii; i++)
            {
                for (int j = 0; j < nrLinii; j++)
                {
                    if (matriceSuprafata[i, j] == 2)
                    {
                        pozitieTurnuri[z].x = j;
                        pozitieTurnuri[z].y = i;
                        z++;
                    }
                }
            }
        }

        private void Initializare_HP_Inamici()
        {           
            for(int i=0;i<nrInamici;i++)
            {
                baraHP.Add(new ProgressBar());
                baraHP[i].Parent = pictureBox3;
                baraHP[i].Size = new Size(275, 20);
                baraHP[i].Location = new Point(75, i*30);
                baraHP[i].Maximum = 100;
                baraHP[i].Minimum = 0;
                baraHP[i].Step = -10;
                baraHP[i].Value = 100;
                
                numeHPInamic.Add(new Label());
                numeHPInamic[i].Parent = this.pictureBox3;
                numeHPInamic[i].Size = new Size(70, 13);
                numeHPInamic[i].Location = new Point(1, i * 30);
                numeHPInamic[i].Text = "Inamicul " + i.ToString() + ":";
                numeHPInamic[i].ForeColor = Color.White;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Creare suprafata de joc
            //Fiecare patrat are 30 de pixeli
            Graphics g = e.Graphics;
            Pen penW = new Pen(Color.White);
            SolidBrush sb = new SolidBrush(Color.Orange);
             for(int i=0;i<12;i++)
            {
                for(int j=0;j<12;j++)
                {
                    //Drum-inamici
                    if (matriceSuprafata[i, j] == 1)
                    {
                        g.FillRectangle(new SolidBrush(Color.White), j * dimensiunePatrat + 1, i * dimensiunePatrat + 1, dimensiunePatrat-1, dimensiunePatrat-1);
                    }
                    //Baze
                    if (matriceSuprafata[i, j] == -1)
                    {
                        g.DrawImage(Image.FromFile("../../../castel.jpg"), j * this.dimensiunePatrat + 1, i * this.dimensiunePatrat + 1, this.dimensiunePatrat - 1, this.dimensiunePatrat - 1);
                    }
                    //Loc turnuri
                    if (matriceSuprafata[i, j] == 2)
                    {
                        g.FillRectangle(new SolidBrush(Color.Green), j * dimensiunePatrat + 1, i * dimensiunePatrat + 1, dimensiunePatrat - 1, dimensiunePatrat - 1);
                    }
                }
            }
            for(int k=0;k<nrTurnuri;k++)
            {
                if(turn[k]!=null)
                {
                    g.DrawImage(Image.FromFile("../../../turn.jpg"), this.pozitieTurnuri[k].x * this.dimensiunePatrat + 1, this.pozitieTurnuri[k].y * this.dimensiunePatrat + 1, this.dimensiunePatrat - 1, this.dimensiunePatrat - 1);
                    g.DrawEllipse(new Pen(Color.Purple), pozitieTurnuri[k].x * dimensiunePatrat - turn[k].razaArctiune/2 + dimensiunePatrat/2 , pozitieTurnuri[k].y * dimensiunePatrat - turn[k].razaArctiune/2 + dimensiunePatrat/2 , turn[k].razaArctiune, turn[k].razaArctiune);
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            Graphics g = pictureBox1.CreateGraphics();
            Random r = new Random();
            for (int i = 0; i < nrInamici; i++)
            {
                SolidBrush sr = new SolidBrush(Color.Blue);
                g.FillEllipse(sr, inamic[i].pozitie.X, inamic[i].pozitie.Y, inamic[i].raza, inamic[i].raza);

            }
            Schimbare_De_Directie();
        }

        private void Schimbare_De_Directie()
        {
            //Pentru schimbare de directie
            Random r = new Random();
            for (int i = 0; i < nrInamici; i++)
            {
                if (inamic[i].pozitie.X == drumInamici[nrPozitieInamic[i]].x * dimensiunePatrat + inamic[i].pRx && inamic[i].pozitie.Y == drumInamici[nrPozitieInamic[i]].y * dimensiunePatrat + inamic[i].pRy)
                {
                    nrPozitieInamic[i]++;
                }
                //Pentru miscare pe axa Ox
                if (inamic[i].pozitie.X < drumInamici[nrPozitieInamic[i]].x * dimensiunePatrat + inamic[i].pRx)
                {
                    inamic[i].pozitie.X += r.Next(1, 4);
                }
                if (inamic[i].pozitie.X > drumInamici[nrPozitieInamic[i]].x * dimensiunePatrat + inamic[i].pRx)
                {
                    inamic[i].pozitie.X -= r.Next(1, 4);
                }
                //Pentru miscare pe axa Oy
                if (inamic[i].pozitie.Y < drumInamici[nrPozitieInamic[i]].y * dimensiunePatrat + inamic[i].pRy) 
                {
                    inamic[i].pozitie.Y += r.Next(1, 4);
                }
                if (inamic[i].pozitie.Y > drumInamici[nrPozitieInamic[i]].y * dimensiunePatrat + inamic[i].pRy)
                {
                    inamic[i].pozitie.Y -= r.Next(1, 4);
                }
                if (inamic[i].pozitie.X / dimensiunePatrat==drumInamici[DrumInamici.lungime].x && inamic[i].pozitie.Y/dimensiunePatrat==drumInamici[DrumInamici.lungime].y)
                {
                    inamic.Remove(inamic[i]);
                    nrInamici--;
                    nrPozitieInamic.Remove(nrPozitieInamic[i]);
                    baraHP[i].Visible = false;
                    baraHP.Remove(baraHP[i]);
                    numeHPInamic[i].Visible = false;
                    numeHPInamic.Remove(numeHPInamic[i]);
                    progressBar1.PerformStep();
                    if(progressBar1.Value==0)
                    {
                        timer1.Enabled = false;
                        timer2.Enabled = false;
                        MessageBox.Show("Ai pierdut cu un scor de: " + score);
                        this.Close();
                    }
                    i--;
                    refreshHP();
                }
            }
        }

        private void Atac_Turnuri()
        {
            for (int i = 0; i < nrTurnuri; i++)
                if (turn[i] != null) //Daca exista turnul verifica coliziune 
                {
                    //MessageBox.Show((pozitieCurenta[0].y/dimensiunePatrat).ToString());
                    for (int z = 0; z < nrInamici; z++)
                    {
                        int pozX = inamic[z].pozitie.X / dimensiunePatrat;
                        int pozY = inamic[z].pozitie.Y / dimensiunePatrat;
                        //MessageBox.Show(pozY.ToString() + " " + turn[i].patrate[0].y.ToString());
                        if (pozX >= turn[i].patrate[0].x && pozX <= turn[i].patrate[0].x + 2 && pozY >= turn[i].patrate[0].y && pozY <= turn[i].patrate[0].y + 2)
                        {
                            for (int j = 0; j < turn[i].nrAcoperire; j++) // Verifica pentru fiecare patrat pe care il are in aria de acoperire Daca contine vreun inamic
                            {
                                if (pozX == turn[i].patrate[j].x && pozY == turn[i].patrate[j].y)
                                {
                                    proiectiles(pozitieTurnuri[i].x * dimensiunePatrat + dimensiunePatrat / 2, pozitieTurnuri[i].y * dimensiunePatrat + dimensiunePatrat / 2, inamic[z].pozitie.X, inamic[z].pozitie.Y, z);
                                    break;
                                }
                            }
                        }
                    }
                }
        }

        private void proiectiles(int x,int y,int destx,int desty,int z)
        {
            timer2.Enabled = false;
            Graphics g = pictureBox1.CreateGraphics();
            while(x!=destx && y!=desty)
            {
                g.FillEllipse(new SolidBrush(Color.Yellow), x, y, 5, 5);
                if (destx>x)
                {
                    x++;
                }
                else if(destx<x)
                {
                    x--;               
                }
                if(desty>y)
                {
                    y++;
                }
                else if(desty<y)
                {
                    y--;
                }
            }
            baraHP[z].PerformStep();
            if(baraHP[z].Value==0)
            {
                inamic.Remove(inamic[z]);
                nrPozitieInamic.Remove(nrPozitieInamic[z]);
                baraHP[z].Visible = false;
                baraHP.Remove(baraHP[z]);
                numeHPInamic[z].Visible = false;
                numeHPInamic.Remove(numeHPInamic[z]);
                nrInamici--;
                z--;
                refreshHP();
                score += 10;
            }
            timer2.Enabled = true;
        }
        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            turn[nrTurn] = new TurnTier1(1, 4 * dimensiunePatrat );
            int[] v1 = { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            int[] v2 = { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
            for(int i=0;i<9;i++)
            {
                int x = pozitieTurnuri[nrTurn].x + v1[i];
                int y = pozitieTurnuri[nrTurn].y + v2[i];              
                if(matriceSuprafata[y,x] == 1 )
                {
                    //introducere patrate din campul de actiune
                    //TREBUIE AFISARE      
                    turn[nrTurn].patrate[turn[nrTurn].nrAcoperire].x = pozitieTurnuri[nrTurn].x + v1[i];
                    turn[nrTurn].patrate[turn[nrTurn].nrAcoperire].y = pozitieTurnuri[nrTurn].y + v2[i];
                    turn[nrTurn].nrAcoperire++;            
                }
            }
            timer1.Enabled = true;
            pictureBox2.Visible = false;

        }
        
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox2.Visible == true)
            {
                //Inchidere meniu de cumparare turnuri dc se apasa cand e deja deschis
                pictureBox2.Visible = false;
                timer1.Enabled = true;
            }
            else
            {
                int _x, _y;
                _x = e.X;
                _y = e.Y;
                //Verificare dc click-ul mousului e pe suprafata unui turn
                for (int i = 0; i < nrTurnuri; i++)
                {

                    if (_x > pozitieTurnuri[i].x * dimensiunePatrat && _x < (pozitieTurnuri[i].x + 1) * dimensiunePatrat)
                    {
                        if (_y > pozitieTurnuri[i].y * dimensiunePatrat && _y < (pozitieTurnuri[i].y + 1) * dimensiunePatrat)
                        {
                            //Pauza la joc pentru a deschide picturebox2 ( meniu de cumparare turnuri ) 
                            timer1.Enabled = false;
                            pictureBox2.Location = new System.Drawing.Point(pozitieTurnuri[i].x * dimensiunePatrat, pozitieTurnuri[i].y * dimensiunePatrat - 75);
                            nrTurn = i;
                            pictureBox2.Visible = true;
                            break;
                        }
                    }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Atac_Turnuri();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int x = r.Next(0, dimensiunePatrat - 10);
            int y = r.Next(0, dimensiunePatrat - 10);
            inamic.Add(new Inamici(drumInamici[0].x + x, drumInamici[0].y + y, 10, 100, x, y, r.Next(1, 4)));
            nrPozitieInamic.Add(1);
            baraHP.Add(new ProgressBar());
            int i = baraHP.Count - 1;
            baraHP[i].Parent = pictureBox3;
            baraHP[i].Size = new Size(275, 20);
            baraHP[i].Location = new Point(75, i * 30);
            baraHP[i].Maximum = 100;
            baraHP[i].Minimum = 0;
            baraHP[i].Step = -2;
            baraHP[i].Value = 100;
            nrInamici++;

            numeHPInamic.Add(new Label());
            numeHPInamic[i].Parent = this.pictureBox3;
            numeHPInamic[i].Size = new Size(70, 13);
            numeHPInamic[i].Location = new Point(1, i * 30);
            numeHPInamic[i].Text = "Inamicul " + i.ToString() + ":";
            numeHPInamic[i].ForeColor = Color.White;
        }

        private void Suprafata_Load(object sender, EventArgs e)
        {

        }

        private void refreshHP()
        {
            for (int i = 0; i < this.baraHP.Count; i++)
            {
                numeHPInamic[i].Location = new Point(1, i * 30);
                baraHP[i].Location = new Point(75, i * 30);
            }
        }
    }
}
//pozitieTurnuri[i].x * dimensiunePatrat + 1 - turn[i].razaArctiune / 5 
//pozitieTurnuri[i].x*dimensiunePatrat + 1 -turn[i].razaArctiune + dimensiunePatrat*1.41f
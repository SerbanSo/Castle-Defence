using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestat
{
    class Inamici
    {
        int _raza;
        int _healthPoints;
        public int pRx;
        public int pRy;
        public int HeathPoints
        {
            set
            {
                _healthPoints = value;
            }
            get
            {
                return _healthPoints;
            }
        }
        public int raza
        {
            get
            {
                return _raza;
            }
        }
        public int ok;

        int viteza;
        public int Viteza
        {
            get
            {
                return viteza;
            }
        }

        public struct pozitieCurenta
        {
            int x;
            public int X
            {
                set
                {
                    x = value;
                }
                get
                {
                    return x;
                }
            }

            int y;
            public int Y
            {
                set
                {
                    y = value;
                }
                get
                {
                    return y;
                }
            }
        }
        public pozitieCurenta pozitie;

        public Inamici(int _x, int _y, int _raza, int _healthPoints, int pRx, int pRy, int viteza)
        {
            pozitie.X = _x;
            pozitie.Y = _y;
            this._raza = _raza;
            this._healthPoints = _healthPoints;
            this.pRx = pRx;
            this.pRy = pRy;
            this.viteza = viteza;
            ok = 1;
        }
    }
}

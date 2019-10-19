using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestat
{
    class TurnTier1
    {
        int _damage;
        float _razaActiune;        
        public float razaArctiune
        {
            get
            {
                return _razaActiune;
            }
        }
        int _nrAcoperire = 0;
        public int nrAcoperire
        {
            set
            {
                _nrAcoperire = value;
            }
            get
            {
                return _nrAcoperire;
            }
        }

        public struct patrateAcoperite
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
        };

        public patrateAcoperite[] patrate = new patrateAcoperite[9];
        public TurnTier1(int damage,float razaActiune)
        {
            _damage = damage;
            _razaActiune = razaActiune;
        }
    }
}

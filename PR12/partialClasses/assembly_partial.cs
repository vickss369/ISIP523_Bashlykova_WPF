using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12
{
    public partial class assembly_
    {
        public decimal TotalPrice
        {
            get
            {
                return partassembly_?.Where(p => p.basepart_ != null).Sum(p => p.basepart_.price) ?? 0;
            }
        }

        public int PartsCount
        {
            get
            {
                return partassembly_?.Count ?? 0;
            }
        }

        public string CreatedDateFormatted
        {
            get
            {
                return DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            }
        }
    }
}

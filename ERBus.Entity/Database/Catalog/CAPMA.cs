using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("CAPMA")]
    public class CAPMA : EntityBase
    {
        [Key]
        [Column("ID")]
        [StringLength(50)]
        public string ID { get; set; }

        [Column("LOAIMA")]
        [StringLength(18)]
        public string LOAIMA { get; set; }

        [Column("NHOMMA")]
        [StringLength(18)]
        public string NHOMMA { get; set; }

        [Column("GIATRI")]
        [StringLength(10)]
        public string GIATRI { get; set; }

        [Column("UNITCODE")]
        [StringLength(10)]
        public string UNITCODE { get; set; }

        public string GenerateNumber()
        {
            var result = "";
            int number;
            var length = GIATRI.Length;
            if (int.TryParse(GIATRI, out number))
            {
                result = string.Format("{0}", number + 1);
                result = AddString(result, length, "0");
            }
            return result;
        }
        public string GenerateChar()
        {
            var result = "";
            char newChar = Convert.ToChar(GIATRI);
            newChar++;
            if ((int)newChar > 90)
            {
                return result;
            }
            result = newChar.ToString();
            return result;
        }
        public string AddString(string input, int length, string character)
        {
            var result = input;
            while (result.Length < length)
            {
                result = string.Format("{0}{1}", character, result);
            }
            return result;
        }
    }
}

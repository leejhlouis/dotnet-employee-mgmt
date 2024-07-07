using System.ComponentModel.DataAnnotations;

namespace SMKaryawan.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string NIK { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string Alamat { get; set; } = string.Empty;

        [Display(Name = "Tanggal Lahir")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TanggalLahir { get; set; }

        [Display(Name = "Jenis Kelamin")]
        public string JenisKelamin { get; set; } = string.Empty;

        public string Departemen { get; set; } = string.Empty;
        public string Jabatan { get; set; } = string.Empty;
    }
}

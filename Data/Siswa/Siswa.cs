namespace TestingSQLITE.Data.Siswa
{
	internal class Siswa : ISiswa
	{
		public int Id { get; set; }
		public string Nama { get; set; }
		public string Alamat { get; set; }
		public string Kelas { get; set; }



		public bool UpdateSiswa(string nama, string alamat, string kelas)
		{
			nama = string.IsNullOrEmpty(nama) ? Nama : nama;
			alamat = string.IsNullOrEmpty(alamat) ? Alamat : alamat;
			kelas = string.IsNullOrEmpty(kelas) ? Kelas : kelas;
			var query = $"UPDATE siswa SET nama = '{nama}', alamat = '{alamat}', kelas = '{kelas}' WHERE id = {Id}";
			return Db.NonQuery(query);
		}
		public bool DeleteSiswa()
		{
			var query = $"DELETE FROM siswa WHERE id = {Id}";
			return Db.NonQuery(query);
		}
	}
}

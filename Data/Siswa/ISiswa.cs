namespace TestingSQLITE.Data.Siswa
{
	internal interface ISiswa
	{
		bool UpdateSiswa(string nama, string alamat, string kelas);
		bool DeleteSiswa();
	}
	internal interface ISiswaService
	{
		List<Siswa> GetAllSiswa();
		bool InsertSiswa(string nama, string alamat, string kelas);
	}
}

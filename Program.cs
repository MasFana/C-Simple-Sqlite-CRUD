using System;
using System.Collections.Generic;
using System.Linq;
using TestingSQLITE.Data.Siswa;

namespace TestingSQLITE
{

	public class Program
	{
		public static void Main(string[] args)
		{
			var siswaService = new SiswaService();
			var menuHandler = new MenuHandler(siswaService);

			while (true)
			{
				menuHandler.DisplayMenu();
			}
		}
	}

	internal class MenuHandler
	{
		private readonly SiswaService _siswaService;
		private List<Siswa> _listSiswa;

		public MenuHandler(SiswaService siswaService)
		{
			_siswaService = siswaService;
			_listSiswa = _siswaService.GetAllSiswa();
		}

		public void DisplayMenu()
		{
			Console.Clear();
			Console.WriteLine("====================================");
			Console.WriteLine("         Siswa CRUD Menu            ");
			Console.WriteLine("====================================");
			Console.WriteLine("1. Create Siswa");
			Console.WriteLine("2. Read Siswa");
			Console.WriteLine("3. Update Siswa");
			Console.WriteLine("4. Delete Siswa");
			Console.WriteLine("5. Exit");
			Console.WriteLine("====================================");
			Console.Write("Pilih menu: ");

			string option = Console.ReadLine() ?? string.Empty;

			Console.Clear();
			switch (option)
			{
				case "1":
					CreateSiswa();
					break;
				case "2":
					ReadSiswa();
					break;
				case "3":
					ReadSiswa();
					UpdateSiswa();
					break;
				case "4":
					ReadSiswa();
					DeleteSiswa();
					break;
				case "5":
					Environment.Exit(0);
					break;
				default:
					Console.WriteLine("Invalid Menu. Silahkan coba lagi.");
					break;
			}
			Console.ReadLine();
		}

		private void CreateSiswa()
		{
			Console.WriteLine("\n Create Siswa \n");
			Console.Write("Nama: ");
			string nama = Console.ReadLine() ?? string.Empty;
			Console.Write("Alamat: ");
			string alamat = Console.ReadLine() ?? string.Empty;
			Console.Write("Kelas: ");
			string kelas = Console.ReadLine() ?? string.Empty;

			if (_siswaService.InsertSiswa(nama, alamat, kelas))
			{
				Console.WriteLine("Berhasil menambahkan data.");
				RefreshSiswaList();
			}
			else
			{
				Console.WriteLine("Gagal insert data.");
			}
		}

		private void ReadSiswa()
		{
			Console.WriteLine(new string('=', 64));
			Console.WriteLine($"|{"Id",-4}|{"Nama",-18}|{"Alamat",-28}|{"Kelas",-9}|");
			Console.WriteLine(new string('=', 64));
			foreach (var item in _listSiswa)
			{
				Console.WriteLine($"|{item.Id,-4}|{item.Nama,-18}|{item.Alamat,-28}|{item.Kelas,-9}|");
				Console.WriteLine(new string('-', 64));
			}
		}

		private void UpdateSiswa()
		{
			Console.WriteLine("\n Update Siswa \n");
			Console.Write("ID: ");
			if (!int.TryParse(Console.ReadLine(), out int id) || !_listSiswa.Any(x => x.Id == id))
			{
				Console.WriteLine("Data tidak ditemukan.");
				return;
			}

			Console.Write("Nama: ");
			string nama = Console.ReadLine() ?? string.Empty;
			Console.Write("Alamat: ");
			string alamat = Console.ReadLine() ?? string.Empty;
			Console.Write("Kelas: ");
			string kelas = Console.ReadLine() ?? string.Empty;

			var currentSiswa = _listSiswa.FirstOrDefault(x => x.Id == id);

			if (currentSiswa!.UpdateSiswa(nama, alamat, kelas))
			{
				Console.WriteLine("Berhasil update data.");
				RefreshSiswaList();
			}
			else
			{
				Console.WriteLine("Gagal update data.");
			}
		}

		private void DeleteSiswa()
		{
			Console.WriteLine("\n Delete Siswa \n");
			Console.Write("ID: ");
			if (!int.TryParse(Console.ReadLine(), out int id) || !_listSiswa.Any(x => x.Id == id))
			{
				Console.WriteLine("Data tidak ditemukan.");
				return;
			}
			
			var currentSiswa = _listSiswa.FirstOrDefault(x => x.Id == id);

			if (currentSiswa.DeleteSiswa())
			{
				Console.WriteLine("Berhasil delete data.");
				RefreshSiswaList();
			}
			else
			{
				Console.WriteLine("Gagal delete data.");
			}
		}

		private void RefreshSiswaList()
		{
			_listSiswa = _siswaService.GetAllSiswa();
		}
	}

}
namespace Server.Utilities.Handler;

public class CheckDaysHandler
{
    public int Get(DateTime leaveStart, DateTime leaveEnd) // yang dimasukkan dd/mm/yyyy yang sesuai datetime mm/dd/yyyy
    {
        List<DateTime> holidays = new List<DateTime>
        {
            new DateTime(2023, 1, 1),   // Tahun Baru
            new DateTime(2023, 1, 22),  // Tahun Baru Imlek (Kongzili)
            new DateTime(2023, 2, 18),  // Isra' Mikraj Nabi Muhammad SAW
            new DateTime(2023, 3, 22),  // Nyepi Tahun Baru Saka 1945
            new DateTime(2023, 4, 7),   // Wafat Isa Almasih
            new DateTime(2023, 4, 22),  // Idul Fitri 1
            new DateTime(2023, 4, 23),  // Idul Fitri 2
            new DateTime(2023, 5, 1),   // Hari Buruh Internasional
            new DateTime(2023, 5, 18),  // Kenaikan Isa Almasih
            new DateTime(2023, 6, 1),   // Hari Lahir Pancasila
            new DateTime(2023, 6, 4),   // Hari Raya Waisak
            new DateTime(2023, 6, 29),  // Idul Adha
            new DateTime(2023, 7, 19),  // Tahun Baru Islam 1445 Hijriah
            new DateTime(2023, 8, 17),  // Kemerdekaan RI
            new DateTime(2023, 9, 28),  // Maulid Nabi Muhammad SAW
            new DateTime(2023, 12, 25), // Hari Raya Natal
            // Tambahkan lebih banyak tanggal hari libur jika diperlukan
        };

        // Inisialisasi jumlah hari cuti
        int leaveDays = 0;

        // Loop melalui setiap tanggal dalam rentang cuti
        for (DateTime currentDate = leaveStart; currentDate <= leaveEnd; currentDate = currentDate.AddDays(1))
        {
            // Cek apakah tanggal saat ini adalah akhir pekan (Sabtu atau Minggu)
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                continue; // Lewati hari akhir pekan
            }

            // Cek apakah tanggal saat ini adalah hari libur nasional
            if (holidays.Contains(currentDate))
            {
                continue; // Lewati hari libur nasional
            }

            // Jika tidak termasuk akhir pekan atau hari libur nasional, tambahkan ke jumlah hari cuti
            leaveDays++;
        }

        return leaveDays;
    }
}
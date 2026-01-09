# Persami Misterius 🌲🔥
**Reorientasi Edukasi Kepanduan melalui Media 3D Adventure-Puzzle**

Proyek ini adalah tugas akhir mata kuliah **Pemrograman Berorientasi Objek (PBO)** - Prodi Pendidikan Multimedia UPI Cibiru. Game ini mengintegrasikan materi SKU Pramuka ke dalam simulasi survival 3D yang imersif.

---

## 🎮 Tentang Game
"Persami Misterius" menceritakan tentang Hamzah, seorang anggota Pramuka yang terpisah dari rombongannya di Hutan Rimbawara. Untuk bertahan hidup dan keluar dari hutan, ia harus menyelesaikan tantangan praktis kepramukaan di bawah ancaman entitas misterius.

### Fitur Utama:
- **Digitalisasi SKU:** Teka-teki berbasis materi Pramuka (Api unggun, Navigasi, Morse).
- **Thermal Survival:** Sistem suhu tubuh yang mengharuskan pemain tetap hangat.
- **Atmospheric Horror:** Lingkungan hutan dinamis dengan sistem kabut dan audio spasial.

---

## 🛠️ Implementasi OOP (Object-Oriented Programming)
Game ini dibangun dengan menerapkan prinsip dasar PBO:
- **Encapsulation:** Perlindungan data pada variabel `currentTemperature` dan `missionProgress`.
- **Inheritance:** Penggunaan `ItemObject.cs` sebagai parent class untuk item Wood, Oil, dan Match.
- **Singleton Pattern:** Digunakan pada `GameManager.cs` untuk kontrol state global.
- **State Pattern:** Mengatur perilaku AI "Shadow Entity" (Idle, Chasing, Retreating).

---

## 📁 Struktur Repositori
```text
Assets/
├── _Scripts/         # Logika C# (Core, Gameplay, UI)
├── Prefabs/          # Objek yang dapat digunakan kembali
├── Scenes/           # Main Menu & Level Gameplay
├── Materials/        # Texture & Visual Styling
└── Docs/             # GDD, TDD, & Laporan Akhir

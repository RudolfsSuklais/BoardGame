# BoardGame

2.5D Board game with multiple players and characters

**Todo list:**

-   [x] Multiple cursors
-   [x] Main menu animations
-   [x] Main menu buttons (start, quit, settings)
-   [x] Character selection screen with animations
-   [x] Settings scene :)
-   [x] Board scene with throwable dice
-   [x] Game logic with multiple players :)
-   [x] Game camera :)
-   [x] Leaderboard scene :)

# 🎲 Dice Board Game (Unity)

Turn-based galda spēle, izstrādāta ar **Unity**, kur spēlētāji met kauliņu, pārvietojas pa laukumu, cīnās savā starpā un sacenšas par uzvaru, balstoties uz **laiku, kauliņu metienu skaitu un punktiem**.

---

## 🧩 Galvenās iespējas

- 🎲 Kauliņa mešanas mehānika ar fiziku  
- 🧭 Laukuma sistēma ar *bounce-back* loģiku  
- ⚔️ Cīņas mehānika, ja spēlētāji nonāk uz viena lauciņa  
- ⏱️ Spēles laika skaitītājs  
- 🏆 Uzvaras ekrāns ar statistiku  
- 📊 **Leaderboard sistēma**, kas saglabājas starp spēlēm  
- ⏸️ Pause menu ar:
  - Continue
  - Settings
  - Leaderboard
  - Main Menu
- 🎵 Audio iestatījumi (Music / SFX)  
- 🖥️ Resolution + Fullscreen iestatījumi  
- 💾 Iestatījumu saglabāšana ar `PlayerPrefs`

---

## 🧮 Punktu sistēma

Punkti tiek aprēķināti, ņemot vērā:

- ⏱️ Spēlē pavadīto laiku  
- 🎲 Kauliņu metienu skaitu  

### Formula
BaseScore = 10000
Score = BaseScore
- (DiceRolls × 150)
- (TimeInSeconds × 5)

Minimum score = 0

Jo **ātrāk** un ar **mazāk metieniem** – jo labāks rezultāts.

---

## 🏆 Leaderboard

Leaderboard dati tiek saglabāti lokāli failā:


### Leaderboard īpašības
- 📈 Kārtots pēc **BestScore** (dilstoši)
- 🕒 Vienāda score gadījumā – pēc pēdējās uzvaras datuma
- 📂 Saglabājas starp spēles palaišanām
- 📊 Pieejams gan **Main Menu**, gan **Game Scene**

---

## ⚙️ Settings

### 🎵 Audio
- Music Volume: `0% / 25% / 50% / 75% / 100%`
- SFX Volume (attiecas uz visiem spēles skaņu avotiem)

### 🖥️ Video
- Ekrāna izšķirtspēja (automātiski ielādē visas pieejamās)
- Fullscreen On / Off

Visi iestatījumi:
- tiek saglabāti ar `PlayerPrefs`
- darbojas **vienoti** gan Main Menu, gan Game Scene

---

## 🎮 Vadība

- 🖱️ Klikšķis uz kauliņa — mest kauliņu  
- ⏸️ Pause poga — aptur spēli  
- 🧭 UI pogas navigācijai pa izvēlnēm  

---

## 🛠️ Izmantotās tehnoloģijas

- **Unity**
- **C#**
- **TextMeshPro**
- **Unity UI**
- **Coroutines**
- **PlayerPrefs**
- **Local File Storage**

---

**Attēli**
<img width="2880" height="1618" alt="boardgame_5" src="https://github.com/user-attachments/assets/5293ebeb-b603-4193-8bc1-76fb988c9c07" />
<img width="2880" height="1620" alt="boardgame_1" src="https://github.com/user-attachments/assets/25bf9505-8ff1-464c-b404-6d2791285974" />
<img width="2880" height="1620" alt="boardgame_2" src="https://github.com/user-attachments/assets/e1a82f3d-c8cd-4f52-a8f1-b7bca1f1bf1d" />
<img width="2880" height="1620" alt="boardgame_3" src="https://github.com/user-attachments/assets/30f3d89d-7842-457a-ae3a-f154ead00fb1" />
<img width="2880" height="1620" alt="boardgame_4" src="https://github.com/user-attachments/assets/db01ed93-28ed-4d30-ae2f-86527de93125" />



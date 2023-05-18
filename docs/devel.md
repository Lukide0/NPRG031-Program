# Development


## Struktura projekt

| Cesta | Popis |
| ----- | ----- |
| Tuif | Terminal framwork|
| Tuif/Dom | Elementy |
| Tuif/Event | Manager událostí |
| Sorter | Aplikace na vizualizaci třídění |
| Sorter/Algo | Algoritmy třídění |

## Tuif

Jednoduchý framework na práci s terminálem. Práce s terminálem je v souboru [`Terminal.cs`](./../Tuif/Terminal.cs). 
- Založen na [ANSI escape kódech](https://en.wikipedia.org/wiki/ANSI_escape_code).
- Všechny komponenty/elementy mají společný [buffer](./../Tuif/Buffer.cs).

### Render

Renderování probíhá v případě, že se někde zavolá `Terminal::RequestRender()` nebo pokud interactivní komponenta/element si vyžádá překreslení.

- Terminál obsahuje 2 vrstvy (front a back).
	- Nejdříve se zobrazí co je v back vrstvě a poté ve front. 

### Terminál

Terminál může být použit 2 způsoby:

- Manuální volání metody `Terminal::Render()`
- Spustění hlavního loopu `Terminal::Loop()`
	- Blocking
		- V každé iteraci bude  terminál čekat na stisknutí tlačítka
	- NonBlocking
		- V každé iteraci se terminál podí zda-li bylo stisknuté tlačítko, pokud nebylo pokračuje
		- Větší využití CPU 

### Dom

Každá kokmponenta/element musí dědit [`Node.cs`](./../Tuif/Dom/Node.cs). Tato základní třída obsahuje pár virtuálních a abstraktních metod:

- `virtual bool Node::HandleKey(ConsoleKeyInfo info, ref Node focusedNode)`
	- Tato metod je zavolána vždy, když je stisknuto tlačítko a komponenta/element je focused
	- Metoda musí vracet, zda-li se mají následující události posílat již rodiči
		- Pokud rodič neexistuje a běží hlavní loop, tak je zastaven
- `virtual void Node::UpdateSize(uint width, uint height)`
-  `abstract void Node::Render(Buffer buff)`
	- Tuto metodu musí implementovat, každá komponenta/element, aby jí mohl terminál vykreslit



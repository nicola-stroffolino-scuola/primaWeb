public class Prenotazione {
    public string? Nome { get; set; }
    public string? Cognome { get; set; }
    public string? Email { get; set; }
    public Sesso Sesso { get; set; }
    public DateOnly DataDiNascita { get; set; }
    public Ruolo Ruolo { get; set; }

    public string GetSesso() {
        string sesso = "";
        switch (Sesso) {
            case Sesso.Maschio: sesso = "Maschio";
            break;
            case Sesso.Femmina: sesso = "Femmina";
            break;
            case Sesso.Null: sesso = "Nullo";
            break;
        }
        return sesso;
    }

    public string GetRuolo() {
        string ruolo = "";
        switch (Ruolo) {
            case Ruolo.Studente: ruolo = "Studente";
            break;
            case Ruolo.Prof: ruolo = "Prof";
            break;
            case Ruolo.Genitore: ruolo = "Genitore";
            break;
        }
        return ruolo;
    }
}

public enum Sesso {
    Maschio = 0,
    Femmina = 1,
    Null = 2
}

public enum Ruolo {
    Studente = 0,
    Prof = 1,
    Genitore = 2
}
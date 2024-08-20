### Autore - Lorenzo Alessi

# PHOTOSI ORDERS

Microservizio ordini della prova pratica di Photosi.

Espone delle API REST chiamate dall'api gateway per effettuare delle CRUD operations.

## PREREQUISITI

1. **.Net 8**
2. **PostgreSQL 16**
3. **IDE per l'avvio del progetto (Rider / Visual Studio)**

## AVVIO DEL PROGETTO

I file di properties appsettings e appsettings.Development.json presentano una sezione
**"ConnectionStrings"** per configurare il database.

| Proprietà  | Tipo   | Descrizione             |
|------------|--------|-------------------------|
| PostgreSql | string | Connessione al database |

**N.B. Non dovranno essere fatte operazioni direttamente a questo servizio dato che
compito dell'api gateway**
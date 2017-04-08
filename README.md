# Project Location API
   This is a code which I made and donated to my actual work (this month: 04/2017). This API will let users using android to register their location during inspections of government constructions sites.
 Of course, the user repository was changed for security reasons.

# Tables

| *FIS_LOCALIZACAO* |*TYPE* | *Comments* |
| --------|---------|-------|
| FISLOCA_ID*  | int(11)| Primary Key |
| LATITUDE* | decimal(10,8)| Latitude GPS |
| LONGITUDE* | decimal(11,8)| Longitude GPS |
| PRECISAO* | decimal(10,)| GPS Precision |
| DATA_CRIACAO* | timestamp | Date of the inspection
| DESC_LOCAL* | varchar(255) | Quick description of the place |
| DESC_OBSERVACAO | varchar(1000) | Free espace for the user |
| INDR_TIPOLANCAMENTO* | tinyint(4)| Type of registry: 0 - Web, 1 - Mobile, 2 - Manual|
| GERUSUA_ID* | int(11)| Primary key of user |

*Means Not Null

Look for the file "TABELAS_MYSQL.sql", to create an demo on mysql.
   
## Constraints

* The user *can't* delete his inspection after 5 days.

## Swagger

I used swagger to help understand what is possible. 

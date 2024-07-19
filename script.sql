create database pitangvac;	

create table tb_paciente ( 
	id_paciente int identity, 
	dsc_nome varchar(max) NOT NULL,
	lgn_paciente VARCHAR(50) NOT NULL,
	dsc_email varchar(max) NOT NULL,
	psw_hash VARBINARY(MAX) NOT NULL,
    psw_salt VARBINARY(MAX) NOT NULL,
	dat_nascimento datetime NOT NULL, 
	dat_criacao datetime NOT NULL, 
	constraint PK_TB_PACIENTE primary key (id_paciente) 
)

create table tb_agendamento ( 
	id_agendamento int identity, 
	id_paciente int not null, 
	dat_agendamento date not null, 
	hor_agendamento time not null, 
	dsc_status varchar(50) not null, 
	dat_criacao datetime not null, 
	constraint PK_TB_AGENDAMENTO primary key (id_agendamento) 
);

alter table tb_agendamento 
add constraint fk_agendamento_paciente foreign key (id_paciente) references dbo.tb_paciente (id_paciente);
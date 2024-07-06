create table employees (
	id serial primary key,
	fullname varchar(255) not null,
	email varchar (255) not null,
	password varchar(255) not null,
	salt varchar(255) not null, 
	subdivision integer not null,
	position integer not null, 
	status boolean not null,
	people_partner_id integer,
	out_of_office_balance integer not null,	
	constraint fk_people_partner foreign key(people_partner_id) references employees(id) on delete set null
);

create table leave_requests(
	id serial primary key,
	employee_id integer not null,
	absense_reason integer not null,
	start_date date not null,
	end_date date not null,
	comment text,
	status integer not null default 0,
	constraint fk_leave_request_employee foreign key(employee_id) references employees(id) on delete cascade
);

create table approval_requests(
	id serial primary key,
	approver_id integer not null,
	leave_request_id integer not null unique,
	status integer not null default 0,
	comment text,
	constraint fk_approval_request_employee foreign key(approver_id) references employees(id) on delete cascade,
	constraint fk_approval_request_leave_request foreign key(leave_request_id) references leave_requests(id) on delete cascade
);

create table projects(
	id serial primary key,
	project_type integer not null,
	start_date date not null,
	end_date date,
	comment text,
	status boolean not null
);

create table employee_projects (
    employee_id integer NOT NULL,
    project_id integer NOT NULL,
    primary key (employee_id, project_id),
    constraint fk_employee_projects_employee foreign key(employee_id) references employees(id) on delete cascade,
    constraint fk_employee_projects_project foreign key(project_id) references projects(id) on delete cascade
);

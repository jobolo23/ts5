create table wiki(
    id int Primary key auto_increment,
    created Datetime,
    modified datetime,
    createuser_id int references users,
    lastuser_id int references users,
    title varchar(50),
    path varchar(700)
    );    
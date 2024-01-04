1. Установка java в wsl Ubuntu

1.1 sudo apt update


1.2 sudo apt install default-jre



2. Установка confluent platform
  2.1. Скачать по ссылке нужную версию платформы,
Я использую community-6.2.0
https://www.confluent.io/previous-versions/
 
  2.2 Скопировать в wsl Ubuntu, в текущую папку(~)
  Команда*:

    sudo cp //mnt/c/Users/Админ/Desktop/Kafka/confluent-community-6.2.0.tar .
  2.3 Распокавать архив 
  Команда:

    tar -xf confluent-community-6.2.0.tar
  2.4 Проверить появления папки с платформой confluent-6.2.0
  Команда:

    ls
  2.5 Добавить переменную среды CONFLUENT_HOME и путь директорию bin confluent-платформы в переменную среды PATH
  Команда**:

    export CONFLUENT_HOME=~/confluent-6.2.0
    export PATH=$PATH:$CONFLUENT_HOME/bin

3. Установка Confluent CLI (оригинальная инструкция https://docs.confluent.io/current/cli/installing.html)
 3.1 Скоприровать архив с CLI в wsl Ubuntu, в текущую папку(~)
 Команда*:

    sudo cp //mnt/c/Users/Админ/Desktop/Kafka/confluent_3.40.0_linux_amd64.tar.gz .
 3.2 Распокавать архив 
 Команда:

    tar -xf confluent_3.40.0_linux_amd64.tar.gz
 3.3 Проверить появление папки confluent
 Команда:

    ls
 3.4 Добавить директорию confluent в переменную среды PATH
 Команда**:

    export PATH=$PATH:~/confluent

4. Установка confluent-hub
4.1 Скачать архив по адресу
https://client.hub.confluent.io/confluent-hub-client-latest.tar.gz;

4.2 Скопировать архив в wsl Ubuntu, в текущую папку
Команда*:

    sudo cp //mnt/c/Users/Админ/Desktop/Kafka/confluent-hub-client-latest.tar.gz .

4.3 Распокавать в директорию платформы confluent platform(в моем случае confluent-6.2.0)
Команда:

    tar -xf confluent-hub-client-latest.tar.gz -C $CONFLUENT_HOME

4.4 Проверить установку 
Команда:

    confluent-hub

5. Установка connector'а
Команда:

    confluent-hub install confluentinc/kafka-connect-jdbc:10.2.0

6. Подготовка базы данных
6.1 Установка sqlite
Команда:

    sudo apt install sqlite3

6.2 Создать таблицу
CREATE TABLE invoices( 
   id INT PRIMARY KEY     NOT NULL,
   title           TEXT    NOT NULL,
   details        CHAR(50),
   billedamt         REAL,
   modified TIMESTAMP DEFAULT (STRFTIME('%s', 'now')) NOT NULL
);

6.3 Наполнить таблицу данными

INSERT INTO invoices (id,title,details,billedamt)  VALUES (1, 'book', 'Franz Kafka', 500.00 );

7. Настройка и запуск connector'a

7.1 Настройка. Создать и заполнить файл
Команды:
> mkdir -p $CONFLUENT_HOME/etc/kafka-connect-jdbc/`
> vi $CONFLUENT_HOME/etc/kafka-connect-jdbc/kafkatest-sqlite.properties`

Настрить конектор:
name=kinaction-test-source-sqlite-jdbc-invoice
connector.class=io.confluent.connect.jdbc.JdbcSourceConnector
tasks.max=1
# SQLite database stored in the file kafkatest.db, use and auto-incrementing column called 'id' to
# detect new rows as they are added, and output to topics prefixed with 'kinaction-test-sqlite-jdbc-', e.g.
# a table called 'invoices' will be written to the topic 'kinaction-test-sqlite-jdbc-invoices'.
connection.url=jdbc:sqlite:kafkatest.db
mode=incrementing
incrementing.column.name=id
topic.prefix=kinaction-test-sqlite-jdbc-

7.2 Запуск конектора
Команды:

> confluent local services connect start 
> confluent local services connect connector config jdbc-source --config $CONFLUENT_HOME/etc/kafka-connect-jdbc/kafkatest-sqlite.properties
> confluent local services connect connector status

8. Запуск потребителя
> $CONFLUENT_HOME/bin/kafka-avro-console-consumer --topic kinaction-test-sqlite-jdbc-invoices --bootstrap-server localhost:9092  --from-beginning



=============================================================================

* Здесь я сохраняю все скачанные архивы //mnt/c/Users/Админ/Desktop/Kafka/
Ниже в примерах можно использовать свою директорию в windows


** При каждом новом запуске wsl, приходится каждый раз добавлять и изменять
переменные среды. Для того чтобы этого не делать, нужно добавить команды для
добавления переменных среды в файл ~/.bashrc:

Команда откроет файл для редактирования:

    sudo vim ~/.bashrc

Добавьте в конце файла все нужные команды:

    export PATH=$PATH:~/confluent
    export CONFLUENT_HOME=~/confluent-6.2.0
    export PATH=$PATH:$CONFLUENT_HOME/bin

=============================================================================

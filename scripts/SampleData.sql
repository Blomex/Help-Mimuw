------ PRZYKŁADOWE PRZEDMIOTY ------
INSERT INTO "Courses"("Name") VALUES ('Rachunek Prawdopodobieństwa');
INSERT INTO "Courses"("Name") VALUES ('Inżynieria Oprogramowania');
INSERT INTO "Courses"("Name") VALUES ('Funkcje Analityczne');
INSERT INTO "Courses"("Name") VALUES ('Algebra');
INSERT INTO "Courses"("Name") VALUES ('Systemy Operacyjne');
INSERT INTO "Courses"("Name") VALUES ('Programowanie Współbieżne');
INSERT INTO "Courses"("Name") VALUES ('Sieci Komputerowe');
INSERT INTO "Courses"("Name") VALUES ('Języki, Automaty i Obliczenia');

------ TROCHĘ TASKSETOW ------
CREATE OR REPLACE FUNCTION InsertSampleTasksets() RETURNS void AS
$$
DECLARE
    course_id INTEGER;
BEGIN
    FOR course_id IN SELECT "Id" FROM "Courses" LOOP
        INSERT INTO "Tasksets"("Type", "Year", "Name", "CourseId") VALUES (0, 2018, 'Egzamin', course_id);
        INSERT INTO "Tasksets"("Type", "Year", "Name", "CourseId") VALUES (0, 2018, 'Egzamin Poprawkowy', course_id);
        INSERT INTO "Tasksets"("Type", "Year", "Name", "CourseId") VALUES (1, 2018, 'Kolokwium', course_id);
        INSERT INTO "Tasksets"("Type", "Year", "Name", "CourseId") VALUES (1, 2018, 'Kolokwium Poprawkowe', course_id);
        INSERT INTO "Tasksets"("Type", "Year", "Name", "CourseId") VALUES (0, 2017, 'Egzamin', course_id);
        INSERT INTO "Tasksets"("Type", "Year", "Name", "CourseId") VALUES (0, 2017, 'Egzamin Poprawkowy', course_id);
    END LOOP;
    RETURN;
END
$$
LANGUAGE 'plpgsql';
SELECT InsertSampleTasksets();

------ ZADANKA ------
CREATE OR REPLACE FUNCTION InsertSampleTasks() RETURNS void AS
$$
DECLARE
    taskset_id INTEGER;
    task_num INTEGER;
    task_id "Tasks"."Id"%type;
BEGIN
    FOR taskset_id  IN SELECT "Id" FROM "Tasksets" LOOP
        FOR task_num IN 1 .. round(random() * 3 + 3) LOOP
            INSERT INTO "Tasks"("Name", "Content", "TasksetId") VALUES ('Zadanie ' || task_num || '.',
                'Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta', taskset_id)
                RETURNING "Id" INTO task_id; -- Ile czasu można spędzić nie widząc, że ma być INTO zamiast AS...
            IF random() > 0.5 THEN
                INSERT INTO "Solutions"("Content", "TaskId") VALUES ('To jest... trywialne', task_id);
            END IF;
        END LOOP;
    END LOOP;
    RETURN;
END
$$
LANGUAGE 'plpgsql';
SELECT InsertSampleTasks();

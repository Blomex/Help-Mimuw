CREATE OR REPLACE FUNCTION Adminize() RETURNS void AS
$$
DECLARE
    imperator_id TEXT;
    user_id TEXT;
BEGIN
    SELECT "Id" FROM "AspNetRoles" WHERE "Name"='IMPERATOR' INTO imperator_id;

    FOR user_id IN SELECT "Id" FROM "AspNetUsers" LOOP
        INSERT INTO "AspNetUserRoles" VALUES (user_id, imperator_id) ON CONFLICT DO NOTHING;
    END LOOP;
    RETURN;
END
$$
LANGUAGE 'plpgsql';

SELECT Adminize();

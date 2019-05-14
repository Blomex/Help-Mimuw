CREATE OR REPLACE FUNCTION Adminize() RETURNS void AS
$$
DECLARE
    imperator_id TEXT;
    moderator_id TEXT;
    trusted_user_id TEXT;
    user_id TEXT;
BEGIN
    SELECT "Id" FROM "AspNetRoles" WHERE "Name"='IMPERATOR' INTO imperator_id;
    SELECT "Id" FROM "AspNetRoles" WHERE "Name"='MODERATOR' INTO moderator_id;
    SELECT "Id" FROM "AspNetRoles" WHERE "Name"='TRUSTED_USER' INTO trusted_user_id;

    FOR user_id IN SELECT "Id" FROM "AspNetUsers" LOOP
        INSERT INTO "AspNetUserRoles" VALUES (user_id, imperator_id) ON CONFLICT DO NOTHING;
        INSERT INTO "AspNetUserRoles" VALUES (user_id, trusted_user_id) ON CONFLICT DO NOTHING;
        INSERT INTO "AspNetUserRoles" VALUES (user_id, moderator_id) ON CONFLICT DO NOTHING;
    END LOOP;
    RETURN;
END
$$
LANGUAGE 'plpgsql';

SELECT Adminize();

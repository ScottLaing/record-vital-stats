CREATE OR REPLACE FUNCTION create_database_if_not_exists(database_name VARCHAR(64)) RETURNS VOID AS $$
BEGIN
  IF NOT EXISTS (SELECT FROM pg_database WHERE datname = database_name) THEN
    EXECUTE FORMAT ($$CREATE DATABASE %I$$, database_name);
  ELSE
    RAISE NOTICE 'Database "%I" already exists.', database_name;
  END IF;
END;
$$ LANGUAGE plpgsql;

SELECT create_database_if_not_exists('Vitals');

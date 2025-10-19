-- Table: public.materialesapp

-- DROP TABLE IF EXISTS public.materialesapp;

CREATE TABLE IF NOT EXISTS public.materialesapp
(
    codproduc character varying(20) COLLATE pg_catalog."default",
    descripcion text COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.materialesapp
    OWNER to postgres;
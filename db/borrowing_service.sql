--
-- PostgreSQL database dump
--

-- Dumped from database version 16.3
-- Dumped by pg_dump version 16.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: sp_issue_book(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_issue_book(p_reader_id integer, p_book_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE books SET is_available = FALSE WHERE id = p_book_id AND is_available = TRUE;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Book not available or not found';
    END IF;

    INSERT INTO borrowings (reader_id, book_id, status)
    VALUES (p_reader_id, p_book_id, 'active');
END;
$$;


ALTER FUNCTION public.sp_issue_book(p_reader_id integer, p_book_id integer) OWNER TO postgres;

--
-- Name: sp_return_book(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_return_book(p_borrowing_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_book_id INT;
BEGIN
    SELECT book_id INTO v_book_id FROM borrowings WHERE id = p_borrowing_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Borrowing not found';
    END IF;

    UPDATE borrowings SET status = 'returned', return_date = CURRENT_TIMESTAMP
    WHERE id = p_borrowing_id;

    UPDATE books SET is_available = TRUE WHERE id = v_book_id;
END;
$$;


ALTER FUNCTION public.sp_return_book(p_borrowing_id integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: book_details; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.book_details (
    book_id integer NOT NULL,
    isbn character varying(20),
    pages integer,
    genre text,
    CONSTRAINT book_details_pages_check CHECK ((pages > 0))
);


ALTER TABLE public.book_details OWNER TO postgres;

--
-- Name: books; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.books (
    id integer NOT NULL,
    title text NOT NULL,
    author_name text NOT NULL,
    is_available boolean DEFAULT true
);


ALTER TABLE public.books OWNER TO postgres;

--
-- Name: books_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.books_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.books_id_seq OWNER TO postgres;

--
-- Name: books_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.books_id_seq OWNED BY public.books.id;


--
-- Name: borrowings; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.borrowings (
    id integer NOT NULL,
    reader_id integer NOT NULL,
    book_id integer NOT NULL,
    borrow_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    return_date timestamp without time zone,
    status text DEFAULT 'active'::text,
    CONSTRAINT borrowings_status_check CHECK ((status = ANY (ARRAY['active'::text, 'returned'::text, 'overdue'::text])))
);


ALTER TABLE public.borrowings OWNER TO postgres;

--
-- Name: borrowings_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.borrowings_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.borrowings_id_seq OWNER TO postgres;

--
-- Name: borrowings_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.borrowings_id_seq OWNED BY public.borrowings.id;


--
-- Name: readers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.readers (
    id integer NOT NULL,
    full_name text NOT NULL,
    email text NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.readers OWNER TO postgres;

--
-- Name: readers_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.readers_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.readers_id_seq OWNER TO postgres;

--
-- Name: readers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.readers_id_seq OWNED BY public.readers.id;


--
-- Name: books id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books ALTER COLUMN id SET DEFAULT nextval('public.books_id_seq'::regclass);


--
-- Name: borrowings id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.borrowings ALTER COLUMN id SET DEFAULT nextval('public.borrowings_id_seq'::regclass);


--
-- Name: readers id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.readers ALTER COLUMN id SET DEFAULT nextval('public.readers_id_seq'::regclass);


--
-- Data for Name: book_details; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.book_details (book_id, isbn, pages, genre) FROM stdin;
\.


--
-- Data for Name: books; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.books (id, title, author_name, is_available) FROM stdin;
5	Маруся Чурай	Ліна Костенко	f
11	Захар Беркут	Іван Франко	f
10	s,dcsdlkn	Тарас Шевченко	f
6	Кобзар	Тарас Шевченко	f
12	12345678	Ліна Костенко	f
13	13пасрпс43342	Тарас Шевченко	t
14	іДЛТввябиядвли	Тарас Шевченко	t
15	ідкт	Іван Франко	t
\.


--
-- Data for Name: borrowings; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.borrowings (id, reader_id, book_id, borrow_date, return_date, status) FROM stdin;
1	1	5	2025-11-24 19:26:57.102501	2025-11-24 21:43:40.681149	returned
2	1	6	2025-11-24 21:51:46.25365	2025-11-24 21:51:58.34808	returned
3	1	10	2025-11-24 21:57:52.899357	2025-11-24 21:58:13.2147	returned
4	3	11	2025-11-24 22:07:13.05965	2025-11-24 22:07:56.579186	returned
5	1	5	2025-11-25 00:15:34.215136	2025-11-25 00:15:55.211067	returned
6	2	11	2025-11-25 00:19:08.511566	2025-11-25 00:19:16.880352	returned
7	1	10	2025-11-25 00:25:58.892595	2025-11-25 00:26:09.922777	returned
8	3	6	2025-11-25 00:29:53.144601	2025-11-25 00:29:59.236176	returned
9	3	12	2025-11-25 00:34:28.822639	2025-11-25 00:34:40.389696	returned
10	1	13	2025-11-25 00:39:16.997041	2025-11-25 00:39:31.08237	returned
11	2	13	2025-11-25 00:40:00.182845	2025-11-25 00:40:06.430586	returned
12	1	13	2025-11-25 13:40:07.428711	2025-11-25 13:40:50.532647	returned
13	3	14	2025-11-25 16:23:05.211919	2025-11-25 21:32:59.159615	returned
14	3	15	2025-11-25 16:23:38.182945	2025-11-26 22:24:30.043695	returned
\.


--
-- Data for Name: readers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.readers (id, full_name, email, created_at) FROM stdin;
1	ervfdv	havrysh.diana@chnu.edu.ua	2025-11-10 21:08:07.02279
2	123456	dgavrish1205@gmail.com	2025-11-18 18:30:43.794745
3	орио о иши	ALDKVN@sjnv	2025-11-24 12:28:30.246087
5	юб 	dgavrish1205@gmail	2025-11-25 13:06:27.646271
\.


--
-- Name: books_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.books_id_seq', 15, true);


--
-- Name: borrowings_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.borrowings_id_seq', 14, true);


--
-- Name: readers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.readers_id_seq', 6, true);


--
-- Name: book_details book_details_isbn_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.book_details
    ADD CONSTRAINT book_details_isbn_key UNIQUE (isbn);


--
-- Name: book_details book_details_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.book_details
    ADD CONSTRAINT book_details_pkey PRIMARY KEY (book_id);


--
-- Name: books books_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_pkey PRIMARY KEY (id);


--
-- Name: borrowings borrowings_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.borrowings
    ADD CONSTRAINT borrowings_pkey PRIMARY KEY (id);


--
-- Name: readers readers_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.readers
    ADD CONSTRAINT readers_email_key UNIQUE (email);


--
-- Name: readers readers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.readers
    ADD CONSTRAINT readers_pkey PRIMARY KEY (id);


--
-- Name: idx_borrowings_book; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_borrowings_book ON public.borrowings USING btree (book_id);


--
-- Name: idx_borrowings_reader; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_borrowings_reader ON public.borrowings USING btree (reader_id);


--
-- Name: book_details book_details_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.book_details
    ADD CONSTRAINT book_details_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id) ON DELETE CASCADE;


--
-- Name: borrowings borrowings_book_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.borrowings
    ADD CONSTRAINT borrowings_book_id_fkey FOREIGN KEY (book_id) REFERENCES public.books(id) ON DELETE CASCADE;


--
-- Name: borrowings borrowings_reader_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.borrowings
    ADD CONSTRAINT borrowings_reader_id_fkey FOREIGN KEY (reader_id) REFERENCES public.readers(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--


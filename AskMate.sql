DROP TABLE IF EXISTS Questions CASCADE;
DROP TABLE IF EXISTS Answers CASCADE;
DROP TABLE IF EXISTS Comment_s CASCADE;
DROP TABLE IF EXISTS Tag CASCADE;
DROP TABLE IF EXISTS Question_tag CASCADE;

CREATE TABLE IF NOT EXISTS Questions(
	question_id SERIAL PRIMARY KEY,
	question_time TIMESTAMP,
	question_viewNumber INT,
	question_voteNumber INT,
	question_title TEXT,
	question_message TEXT,
	question_imageURL TEXT
);


CREATE TABLE IF NOT EXISTS Answers(
	answer_id SERIAL PRIMARY KEY,
	answer_time TIMESTAMP,
    	answer_voteNumber INT,
	question_id INT REFERENCES Questions(question_id) ON DELETE CASCADE,
	answer_message TEXT,
	answer_image TEXT
);

CREATE TABLE IF NOT EXISTS Comment_s(
	comment_id SERIAL PRIMARY KEY,
	question_id INT REFERENCES Questions(question_id) ON DELETE CASCADE,
	answer_id INT REFERENCES Answers(answer_id) ON DELETE CASCADE,
	comment_message TEXT,
	comment_time TIMESTAMP,
	edited_number INT
);

CREATE TABLE IF NOT EXISTS Tags(
	tag_id SERIAL PRIMARY KEY,
	tag_name TEXT
);

CREATE TABLE IF NOT EXISTS Question_tags(
	question_id INT REFERENCES Questions(question_id) ON DELETE CASCADE,
	tag_id INT REFERENCES Tags(tag_id),
	PRIMARY KEY (question_id, tag_id)
);
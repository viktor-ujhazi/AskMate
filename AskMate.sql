CREATE TABLE IF NOT EXISTS Questions(
	question_id SERIAL PRIMARY KEY,
	question_time Date,
	question_viewNumber INT,
	question_voteNumber INT,
	question_title TEXT,
	question_message TEXT,
	question_imageURL TEXT
);


CREATE TABLE IF NOT EXISTS Answers(
	answer_id SERIAL PRIMARY KEY,
	answer_time DATE,
    answer_voteNumber INT,
	question_id INT REFERENCES Questions(question_id),
	answer_message TEXT,
	answer_image TEXT
);
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL,
    access_type ENUM('admin', 'employee') NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insert admin user
INSERT INTO users (username, password, access_type) 
VALUES ('admin', 'admin123', 'admin');

-- Insert employee user
INSERT INTO users (username, password, access_type) 
VALUES ('employee', 'emp123', 'employee');
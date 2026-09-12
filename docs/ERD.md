# Entity Relationship Diagram - GREMIS Platform

## Database Schema

### 1. USER & ROLE MANAGEMENT
```sql
CREATE TABLE users (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  uuid VARCHAR(36) UNIQUE NOT NULL,
  email VARCHAR(255) UNIQUE NOT NULL,
  phone VARCHAR(20),
  full_name VARCHAR(255) NOT NULL,
  password_hash VARCHAR(255),
  role_id INT NOT NULL,
  status ENUM('active', 'inactive', 'suspended') DEFAULT 'active',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (role_id) REFERENCES roles(id)
);

CREATE TABLE roles (
  id INT PRIMARY KEY AUTO_INCREMENT,
  role_name ENUM('migrant_worker', 'family_member', 'teacher_counselor', 'village_admin', 'volunteer', 'psychologist', 'admin') NOT NULL UNIQUE,
  description TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE permissions (
  id INT PRIMARY KEY AUTO_INCREMENT,
  permission_name VARCHAR(100) NOT NULL UNIQUE,
  description TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE role_permissions (
  role_id INT NOT NULL,
  permission_id INT NOT NULL,
  PRIMARY KEY (role_id, permission_id),
  FOREIGN KEY (role_id) REFERENCES roles(id),
  FOREIGN KEY (permission_id) REFERENCES permissions(id)
);
```

### 2. MIGRANT WORKER & FAMILY
```sql
CREATE TABLE migrant_workers (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id BIGINT NOT NULL UNIQUE,
  id_number VARCHAR(50) UNIQUE,
  passport_number VARCHAR(50) UNIQUE,
  country_destination VARCHAR(100),
  contract_start_date DATE,
  contract_end_date DATE,
  occupation VARCHAR(100),
  monthly_income DECIMAL(15,2),
  status ENUM('preparation', 'working', 'return', 'retired') DEFAULT 'working',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE family_members (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  migrant_worker_id BIGINT NOT NULL,
  user_id BIGINT,
  full_name VARCHAR(255) NOT NULL,
  relationship ENUM('spouse', 'child', 'parent', 'sibling', 'other') NOT NULL,
  date_of_birth DATE,
  education_level VARCHAR(100),
  employment_status VARCHAR(100),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (migrant_worker_id) REFERENCES migrant_workers(id),
  FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE villages (
  id INT PRIMARY KEY AUTO_INCREMENT,
  village_name VARCHAR(100) NOT NULL,
  district VARCHAR(100),
  regency VARCHAR(100),
  province VARCHAR(100),
  postal_code VARCHAR(10),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE migrant_worker_village (
  migrant_worker_id BIGINT NOT NULL,
  village_id INT NOT NULL,
  PRIMARY KEY (migrant_worker_id, village_id),
  FOREIGN KEY (migrant_worker_id) REFERENCES migrant_workers(id),
  FOREIGN KEY (village_id) REFERENCES villages(id)
);
```

### 3. EKONOMI KELUARGA (Economic Empowerment)
```sql
CREATE TABLE remittances (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  migrant_worker_id BIGINT NOT NULL,
  amount DECIMAL(15,2) NOT NULL,
  currency VARCHAR(10) DEFAULT 'IDR',
  remittance_date DATE NOT NULL,
  received_date DATE,
  transfer_method ENUM('bank', 'money_transfer', 'cash', 'digital_wallet') NOT NULL,
  description TEXT,
  status ENUM('pending', 'completed', 'failed') DEFAULT 'pending',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (migrant_worker_id) REFERENCES migrant_workers(id)
);

CREATE TABLE family_budgets (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  migrant_worker_id BIGINT NOT NULL,
  month INT NOT NULL,
  year INT NOT NULL,
  total_income DECIMAL(15,2),
  total_expense DECIMAL(15,2),
  savings DECIMAL(15,2),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (migrant_worker_id) REFERENCES migrant_workers(id),
  UNIQUE KEY unique_budget (migrant_worker_id, month, year)
);

CREATE TABLE budget_categories (
  id INT PRIMARY KEY AUTO_INCREMENT,
  category_name VARCHAR(100) NOT NULL UNIQUE,
  category_type ENUM('income', 'expense') NOT NULL,
  description TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE budget_details (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  family_budget_id BIGINT NOT NULL,
  category_id INT NOT NULL,
  amount DECIMAL(15,2) NOT NULL,
  notes TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (family_budget_id) REFERENCES family_budgets(id),
  FOREIGN KEY (category_id) REFERENCES budget_categories(id)
);

CREATE TABLE business_plans (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  migrant_worker_id BIGINT NOT NULL,
  business_name VARCHAR(255) NOT NULL,
  business_type VARCHAR(100),
  description TEXT,
  start_capital DECIMAL(15,2),
  projected_revenue DECIMAL(15,2),
  target_launch_date DATE,
  status ENUM('planning', 'active', 'paused', 'closed') DEFAULT 'planning',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (migrant_worker_id) REFERENCES migrant_workers(id)
);

CREATE TABLE business_progress (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  business_plan_id BIGINT NOT NULL,
  month INT NOT NULL,
  year INT NOT NULL,
  actual_revenue DECIMAL(15,2),
  actual_expense DECIMAL(15,2),
  notes TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (business_plan_id) REFERENCES business_plans(id)
);
```

### 4. PSIKOLOGI REMAJA (Psychological Support)
```sql
CREATE TABLE screening_forms (
  id INT PRIMARY KEY AUTO_INCREMENT,
  form_name VARCHAR(255) NOT NULL UNIQUE,
  form_type ENUM('emotional', 'social', 'adaptation', 'stress', 'general') NOT NULL,
  description TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE screening_questions (
  id INT PRIMARY KEY AUTO_INCREMENT,
  screening_form_id INT NOT NULL,
  question_text TEXT NOT NULL,
  question_order INT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (screening_form_id) REFERENCES screening_forms(id)
);

CREATE TABLE screening_responses (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  family_member_id BIGINT NOT NULL,
  screening_form_id INT NOT NULL,
  response_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (family_member_id) REFERENCES family_members(id),
  FOREIGN KEY (screening_form_id) REFERENCES screening_forms(id)
);

CREATE TABLE screening_answers (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  screening_response_id BIGINT NOT NULL,
  screening_question_id INT NOT NULL,
  answer_value VARCHAR(255),
  answer_score INT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (screening_response_id) REFERENCES screening_responses(id),
  FOREIGN KEY (screening_question_id) REFERENCES screening_questions(id)
);

CREATE TABLE screening_results (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  screening_response_id BIGINT NOT NULL UNIQUE,
  total_score INT,
  result_category ENUM('excellent', 'good', 'fair', 'needs_intervention', 'critical') NOT NULL,
  interpretation TEXT,
  recommendation TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (screening_response_id) REFERENCES screening_responses(id)
);

CREATE TABLE counseling_sessions (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  family_member_id BIGINT NOT NULL,
  psychologist_id BIGINT NOT NULL,
  session_date DATETIME NOT NULL,
  session_type ENUM('online', 'offline') NOT NULL,
  duration_minutes INT,
  status ENUM('scheduled', 'completed', 'cancelled') DEFAULT 'scheduled',
  notes TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (family_member_id) REFERENCES family_members(id),
  FOREIGN KEY (psychologist_id) REFERENCES users(id)
);

CREATE TABLE counseling_records (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  counseling_session_id BIGINT NOT NULL UNIQUE,
  session_summary TEXT,
  issues_discussed TEXT,
  intervention_plan TEXT,
  follow_up_date DATE,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (counseling_session_id) REFERENCES counseling_sessions(id)
);
```

### 5. KOMUNIKASI KELUARGA (Family Connection)
```sql
CREATE TABLE chat_rooms (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  migrant_worker_id BIGINT NOT NULL,
  room_name VARCHAR(255),
  room_type ENUM('one_to_one', 'group_family') NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (migrant_worker_id) REFERENCES migrant_workers(id)
);

CREATE TABLE chat_room_members (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  chat_room_id BIGINT NOT NULL,
  user_id BIGINT NOT NULL,
  joined_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (chat_room_id) REFERENCES chat_rooms(id),
  FOREIGN KEY (user_id) REFERENCES users(id),
  UNIQUE KEY unique_member (chat_room_id, user_id)
);

CREATE TABLE messages (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  chat_room_id BIGINT NOT NULL,
  sender_id BIGINT NOT NULL,
  message_text TEXT,
  media_url VARCHAR(255),
  message_type ENUM('text', 'image', 'video', 'file') DEFAULT 'text',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (chat_room_id) REFERENCES chat_rooms(id),
  FOREIGN KEY (sender_id) REFERENCES users(id)
);

CREATE TABLE child_development_records (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  family_member_id BIGINT NOT NULL,
  record_date DATE NOT NULL,
  height DECIMAL(5,2),
  weight DECIMAL(5,2),
  academic_performance VARCHAR(255),
  behavioral_notes TEXT,
  health_status VARCHAR(255),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (family_member_id) REFERENCES family_members(id)
);
```

### 6. KOLABORASI KOMUNITAS (Community Collaboration)
```sql
CREATE TABLE professionals (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id BIGINT NOT NULL UNIQUE,
  profession_type ENUM('teacher_counselor', 'village_admin', 'volunteer', 'psychologist') NOT NULL,
  specialization VARCHAR(255),
  experience_years INT,
  certification_number VARCHAR(100),
  bio TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE professional_village (
  professional_id BIGINT NOT NULL,
  village_id INT NOT NULL,
  PRIMARY KEY (professional_id, village_id),
  FOREIGN KEY (professional_id) REFERENCES professionals(id),
  FOREIGN KEY (village_id) REFERENCES villages(id)
);

CREATE TABLE forum_posts (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  author_id BIGINT NOT NULL,
  forum_category ENUM('general', 'economic_tips', 'psychology', 'education', 'health') NOT NULL,
  title VARCHAR(255) NOT NULL,
  content TEXT NOT NULL,
  status ENUM('published', 'draft', 'archived') DEFAULT 'published',
  view_count INT DEFAULT 0,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (author_id) REFERENCES users(id)
);

CREATE TABLE forum_comments (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  forum_post_id BIGINT NOT NULL,
  author_id BIGINT NOT NULL,
  comment_text TEXT NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (forum_post_id) REFERENCES forum_posts(id),
  FOREIGN KEY (author_id) REFERENCES users(id)
);

CREATE TABLE collaborative_programs (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  program_name VARCHAR(255) NOT NULL,
  program_description TEXT,
  village_id INT NOT NULL,
  coordinator_id BIGINT NOT NULL,
  program_type ENUM('training', 'counseling', 'support_group', 'workshop') NOT NULL,
  start_date DATE,
  end_date DATE,
  status ENUM('planning', 'active', 'completed') DEFAULT 'planning',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (village_id) REFERENCES villages(id),
  FOREIGN KEY (coordinator_id) REFERENCES professionals(id)
);

CREATE TABLE program_participants (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  collaborative_program_id BIGINT NOT NULL,
  user_id BIGINT NOT NULL,
  participation_status ENUM('registered', 'active', 'completed', 'dropped') DEFAULT 'registered',
  joined_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (collaborative_program_id) REFERENCES collaborative_programs(id),
  FOREIGN KEY (user_id) REFERENCES users(id)
);
```

### 7. DASHBOARD & REPORTING
```sql
CREATE TABLE user_dashboards (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id BIGINT NOT NULL UNIQUE,
  total_remittance_received DECIMAL(15,2) DEFAULT 0,
  family_member_count INT DEFAULT 0,
  latest_psychological_score INT,
  business_status VARCHAR(100),
  active_counseling_sessions INT DEFAULT 0,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE activity_logs (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id BIGINT NOT NULL,
  activity_type VARCHAR(100) NOT NULL,
  description TEXT,
  ip_address VARCHAR(45),
  user_agent TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (user_id) REFERENCES users(id),
  INDEX idx_user_date (user_id, created_at)
);

CREATE TABLE notifications (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id BIGINT NOT NULL,
  notification_type VARCHAR(100) NOT NULL,
  title VARCHAR(255),
  message TEXT,
  related_entity_type VARCHAR(100),
  related_entity_id BIGINT,
  is_read BOOLEAN DEFAULT FALSE,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  read_at TIMESTAMP,
  FOREIGN KEY (user_id) REFERENCES users(id),
  INDEX idx_user_read (user_id, is_read)
);
```

---

## ER Diagram (Simplified View)

```
┌─────────────────────┐
│      USERS          │
├─────────────────────┤
│ id (PK)             │
│ uuid                │
│ email               │
│ full_name           │
│ role_id (FK)        │
└──────────┬──────────┘
           │
      ┌────┴────┬──────────────┬──────────────┐
      │          │              │              │
┌─────▼──────┐┌──▼──────────┐┌▼───────────┐┌──▼────────────┐
│ ROLES      ││MIGRANT_     ││FAMILY_     ││PROFESSIONALS │
│            ││WORKERS      ││MEMBERS     ││               │
└────────────┘└──────┬──────┘└────────────┘└───────────────┘
                     │
        ┌────────────┼────────────┐
        │            │            │
    ┌───▼──────┐ ┌──▼────────┐ ┌──▼────────────┐
    │REMITTANCES│ │BUSINESS_  │ │FAMILY_BUDGETS│
    │           │ │PLANS      │ │              │
    └──────────┘ └───────────┘ └──────────────┘

┌──────────────────────┐
│ SCREENING_FORMS      │
├──────────────────────┤
│ id (PK)              │
│ form_name            │
│ form_type            │
└────────┬─────────────┘
         │
    ┌────▼────────────────┐
    │ SCREENING_RESPONSES │
    │ ├─────────────────  │
    │ │ family_member_id  │
    └────┬────────────────┘
         │
    ┌────▼────────────────┐
    │ SCREENING_RESULTS   │
    │ ├─────────────────  │
    │ │ result_category   │
    │ │ recommendation    │
    └─────────────────────┘

┌─────────────────┐
│ CHAT_ROOMS      │
├─────────────────┤
│ id (PK)         │
│ migrant_id (FK) │
└────────┬────────┘
         │
    ┌────▼─────────────┐
    │ CHAT_ROOM_MEMBERS│
    │ ├────────────    │
    │ │ user_id (FK)   │
    └────┬─────────────┘
         │
    ┌────▼────────┐
    │ MESSAGES    │
    └─────────────┘

┌─────────────────────┐
│ COLLABORATIVE_      │
│ PROGRAMS            │
├─────────────────────┤
│ id (PK)             │
│ village_id (FK)     │
└────────┬────────────┘
         │
    ┌────▼──────────────┐
    │ PROGRAM_           │
    │ PARTICIPANTS       │
    └────────────────────┘
```

---

## Key Relationships Summary

| From | To | Relationship | Type |
|------|-----|------|------|
| users | roles | Many-to-One | Required |
| migrant_workers | users | One-to-One | Required |
| family_members | migrant_workers | Many-to-One | Required |
| family_members | users | Many-to-One | Optional |
| remittances | migrant_workers | Many-to-One | Required |
| business_plans | migrant_workers | Many-to-One | Required |
| screening_responses | family_members | Many-to-One | Required |
| counseling_sessions | family_members | Many-to-One | Required |
| counseling_sessions | users (psychologist) | Many-to-One | Required |
| chat_rooms | migrant_workers | Many-to-One | Required |
| chat_room_members | users | Many-to-One | Required |
| forum_posts | users | Many-to-One | Required |
| collaborative_programs | professionals | Many-to-One | Required |
| program_participants | users | Many-to-One | Required |
| professionals | users | One-to-One | Required |

---

## Database Design Principles Applied

✅ **Normalization**: 3NF (Third Normal Form)
✅ **Data Integrity**: Foreign keys, constraints
✅ **Scalability**: Proper indexing on frequently queried fields
✅ **Flexibility**: Enums for fixed values, TEXT for flexible content
✅ **Audit Trail**: Created_at, updated_at timestamps
✅ **Performance**: Indexes on foreign keys and frequently filtered columns

from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
from AuroraBot.Config import BotConfigManager

config = BotConfigManager.LoadConfig()
connectionString = f"postgresql://{config['DatabaseConfig']['DBUser']}:{config['DatabaseConfig']['DBPass']}@{config['DatabaseConfig']['DBHost']}/{config['DatabaseConfig']['Database']}"
engine = create_engine(connectionString)
Session = sessionmaker(bind=engine)

Base = declarative_base()
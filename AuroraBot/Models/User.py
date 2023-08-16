import datetime

from sqlalchemy import Column, Integer, String, DateTime

from AuroraBot.Models.Base import Base


class User(Base):
    __tablename__ = "Users"
    Id = Column(String, primary_key=True)
    PlayerName = Column(String, nullable=False)
    Gold = Column(Integer, nullable=False, default=0)
    Level = Column(Integer, nullable=False, default=1)
    Experience = Column(Integer, nullable=False, default=0)
    Speed = Column(Integer, nullable=False, default=1)
    Strength = Column(Integer, nullable=False, default=1)
    CreationDate = Column(Integer, nullable=False, default=datetime.datetime.now())

import configparser

from discord.ext import commands
from sqlalchemy import *
from sqlalchemy.orm import sessionmaker

from AuroraBot.Models.User import User


class CreateUserCog(commands.Cog):
    def __init__(self, bot, config: configparser):
        self.bot = bot
        self.config = config

    @commands.command(name="CreateUser")
    async def CreateUser(self, ctx):
        await ctx.reply("Hello, What is your name")
        
        def check(msg):
            return msg.author.id == ctx.author.id and msg.channel.id == ctx.channel.id
        
        try:
            name = await self.bot.wait_for('message', check=check, timeout=60)
        except:
            await ctx.reply("You timed out, try again later")
            return
        try:
            engine = create_engine(f"postgresql://{self.config['DBUser']}:{self.config['DBPass']}@{self.config['DBHost']}/{self.config['Database']}")
        except:
            print(f"Unable to connect to database with postgresql://{self.config['DBUser']}:{self.config['DBPass']}@{self.config['DBHost']}/{self.config['Database']}")
            await ctx.reply("Something went wrong check console for errors")
            return
        try:
            newUser = User(Id=ctx.message.author.id, PlayerName=name.content)
            Session = sessionmaker(bind=engine)
            session = Session()
            session.add(newUser)
            session.commit()
        except Exception as e:
            print(f"Unable to add user: {str(e)}")
            await ctx.reply("Something went wrong check console for errors")
            return
        await name.reply("User created")
def create(db):
    class GameplayData(db.Model):
        __tablename__ = "Gameplay"
        gameLogID = db.Column(db.Integer, primary_key=True, autoincrement=True)
        ParticipantID = db.Column(db.Integer, db.ForeignKey('participant.participantID'))
        TimeStamp = db.Column(db.String)
        Level = db.Column(db.String)
        Category = db.Column(db.String)
        Action = db.Column(db.String)
        Interval = db.Column(db.Float)


    class SummaryData(db.Model):
        __tablename__ = "Summary"
        gameLogID = db.Column(db.Integer, primary_key=True, autoincrement=True)
        ParticipantID = db.Column(db.Integer, db.ForeignKey('participant.participantID'))
        StartTime = db.Column(db.String)
        EndTime = db.Column(db.String)
        Level = db.Column(db.String)
        TotalTime = db.Column(db.Integer)
        TotalCustomers = db.Column(db.Integer)
        PerfectServes = db.Column(db.Integer)
        CorrectServes = db.Column(db.Integer)
        IncorrectServes = db.Column(db.Integer)
        TotalScore = db.Column(db.Integer)

    return GameplayData, SummaryData

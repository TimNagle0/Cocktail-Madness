import datetime
from flask import Blueprint, render_template
from BOFS.util import *
from BOFS.globals import db
from BOFS.admin.util import verify_admin

# The name of this variable must match the folder's name.
Cocktail_Madness = Blueprint('Cocktail_Madness', __name__,
                          static_url_path='/Cocktail_Madness',
                          template_folder='templates',
                          static_folder='static')


def handle_game_post():
    if(request.form['formType'] == 'Continuous'):

        log = db.GameplayData()
        log.ParticipantID = session['participantID']
        log.TimeStamp = request.form['timeStamp']
        log.Level = request.form['level']
        log.Category = request.form['category']
        log.Action = request.form['action']
        log.Interval = request.form['interval']

        db.session.add(log)
        db.session.commit()
        return ""
    else:
        log = db.SummaryData()
        log.ParticipantID = session['participantID']
        log.StartTime = request.form['startTime']
        log.EndTime = request.form['endTime']
        log.Level = request.form['level']
        log.TotalTime = request.form['totalTime']
        log.TotalCustomers = request.form['totalCustomers']
        log.PerfectServes = request.form['perfectServes']
        log.CorrectServes = request.form['correctServes']
        log.IncorrectServes = request.form['incorrectServes']
        log.TotalScore = request.form['totalScore']
        
        db.session.add(log)
        db.session.commit()
        return ""


@Cocktail_Madness.route("/Cocktail_Madness", methods=['POST', 'GET'])
@verify_correct_page
@verify_session_valid
def game_custom():
    if request.method == 'POST':
        return handle_game_post()
    return render_template("Cocktail_Madness.html")


@Cocktail_Madness.route("/fetch_condition")
@verify_session_valid
def fetch_condition():
    return str(session['condition'])



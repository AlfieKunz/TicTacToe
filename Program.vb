Public Class Program
    Public table(2, 2) As String
    Public board(2, 2) As String
    Public winner As String
    Public tempcheck As String
    Public rand1 As Integer
    Public rand2 As Integer
    Public running As Boolean
    Public draw As Boolean
    Public StoredData(9) As String
    Public AbsoluteDepth As Integer

    Private Sub DisplayWin(ByRef winner As String)
        If winner = "Draw" Then
            TextBox1.Text = ("DRAW!!")
        ElseIf winner <> "" Then
            TextBox1.Text = ("We have a winner! (" & winner & ")")
        End If
    End Sub

    Private Sub Slot0_Click(sender As Object, e As EventArgs) Handles Slot0.Click
        If Slot0.Text = "" Then
            Slot0.Text = PlayerChanger.Text
        End If
        table(0, 0) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot1_Click(sender As Object, e As EventArgs) Handles Slot1.Click
        If Slot1.Text = "" Then
            Slot1.Text = PlayerChanger.Text
        End If
        table(0, 1) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot2_Click(sender As Object, e As EventArgs) Handles Slot2.Click
        If Slot2.Text = "" Then
            Slot2.Text = PlayerChanger.Text
        End If
        table(0, 2) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot3_Click(sender As Object, e As EventArgs) Handles Slot3.Click
        If Slot3.Text = "" Then
            Slot3.Text = PlayerChanger.Text
        End If
        table(1, 0) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot4_Click(sender As Object, e As EventArgs) Handles Slot4.Click
        If Slot4.Text = "" Then
            Slot4.Text = PlayerChanger.Text
        End If
        table(1, 1) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot5_Click(sender As Object, e As EventArgs) Handles Slot5.Click
        If Slot5.Text = "" Then
            Slot5.Text = PlayerChanger.Text
        End If
        table(1, 2) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot6_Click(sender As Object, e As EventArgs) Handles Slot6.Click
        If Slot6.Text = "" Then
            Slot6.Text = PlayerChanger.Text
        End If
        table(2, 0) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot7_Click(sender As Object, e As EventArgs) Handles Slot7.Click
        If Slot7.Text = "" Then
            Slot7.Text = PlayerChanger.Text
        End If
        table(2, 1) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    Private Sub Slot8_Click(sender As Object, e As EventArgs) Handles Slot8.Click
        If Slot8.Text = "" Then
            Slot8.Text = PlayerChanger.Text
        End If
        table(2, 2) = PlayerChanger.Text
        DisplayWin(CheckWin(table))
    End Sub

    'Disable to stop simulations.
    Private Sub Form1_Load() Handles Me.Load
        Dim winno As Integer = 0
        Dim drawno As Integer = 0
        Dim lossno As Integer = 0
        While True
            While True
                While True
                    rand1 = CInt((Rnd() * 2))
                    rand2 = CInt((Rnd() * 2))
                    If Me.Controls("Slot" & ((rand1 * 3) + rand2).ToString).Text = "" Then Exit While
                End While
                Me.Controls("Slot" & ((rand1 * 3) + rand2).ToString).Text = "O"
                table(rand1, rand2) = "O"
                If CheckWin(table) <> "" Then
                    Exit While
                End If

                Initialise_MiniMax()
                If CheckWin(table) <> "" Then
                    Exit While
                End If
            End While

            If CheckWin(table) = "X" Then
                winno += 1
            ElseIf CheckWin(table) = "O" Then
                lossno += 1
            Else
                drawno += 1
            End If
            Console.WriteLine("Number of Games Simulated: " & winno + drawno + lossno)
            Console.WriteLine("Number of Wins: " & winno)
            Console.WriteLine("Number of Draws: " & drawno)
            Console.WriteLine("Number of Losses: " & lossno)
            Console.WriteLine("Press any Key to Exit...")
            Console.SetCursorPosition(0, Console.CursorTop - 5)
            For w = 0 To 2
                For e = 0 To 2
                    table(w, e) = ""
                    Me.Controls("Slot" & ((w * 3) + e).ToString).Text = ""
                Next
            Next
            If Console.KeyAvailable() Then
                TextBox1.Text = ""
                TextBox2.Text = ""
                Exit While
            End If
        End While
    End Sub

    Private Sub Button1_Click() Handles Button1.Click
        Randomize()
        running = True
        Do
            rand1 = CInt((Rnd() * 2))
            rand2 = CInt((Rnd() * 2))
            If Me.Controls("Slot" & ((rand1 * 3) + rand2).ToString).Text = "" Then
                running = False
            End If
        Loop While running
        Me.Controls("Slot" & ((rand1 * 3) + rand2).ToString).Text = "X"
        table(rand1, rand2) = "X"
        DisplayWin(CheckWin(table))
    End Sub



    Private Sub Initialise_MiniMax() Handles Button2.Click
        'Computer Plays as X.
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        Dim score As Decimal = 0
        Dim bestscore As Decimal = Integer.MinValue 'Represents the worst move for the computer.
        Dim bestmove As String = ""
        Dim depth As Integer = 1
        Dim RndInt As Integer
        Dim Alpha As Decimal = Integer.MinValue

        Dim BestMoveList As New List(Of String)

        'AbsoluteDepth = 1

        Array.Copy(table, board, 9) 'creates virtual board.
        Randomize()
        Dim watch As Stopwatch = Stopwatch.StartNew()
        For x = 0 To 2
            For y = 0 To 2
                If board(x, y) = "" Then
                    board(x, y) = "X"
                    'For each move possible, it plays it on the temporary board and runs that position through MiniMax.
                    score = MiniMax(board, depth, False, Alpha, Integer.MaxValue)
                    'Alpha = Math.Max(Alpha, score)
                    If score > 0 Then
                        StoredData((x * 3) + y) = ("Move: (" & x & ", " & y & ") Is Winning for AI.")
                    ElseIf score < 0 Then
                        StoredData((x * 3) + y) = ("Move: (" & x & ", " & y & ") Is Losing for AI.")
                    Else
                        StoredData((x * 3) + y) = ("Move: (" & x & ", " & y & ") Is a draw.")
                    End If

                    If score > bestscore Then 'repalces BestScore.
                        BestMoveList.Clear()
                        bestscore = score
                        BestMoveList.Add(x & y)
                    ElseIf score = bestscore Then '50% of the time, BestScore is relaced.
                        BestMoveList.Add(x & y)
                    End If
                    board(x, y) = "" 'undos the last move back to the origiinal position.
                Else
                    StoredData((x * 3) + y) = ("Move: (" & x & ", " & y & ") Is Not Possible")
                End If
                ProgressBar1.Value += 10
            Next
        Next
        watch.Stop()

        bestmove = BestMoveList(Math.Truncate(Rnd() * BestMoveList.Count))

        'Plays the AI's chosen move on the board and displays it.
        Me.Controls("Slot" & ((CInt(bestmove.Substring(0, 1)) * 3) + CInt(bestmove.Substring(1, 1))).ToString).Text = "X"
        table(CInt(bestmove.Substring(0, 1)), CInt(bestmove.Substring(1, 1))) = "X"

        If bestscore > 0 Then
            TextBox1.Text = "Computer Prediction: X is winning."
        ElseIf bestscore < 0 Then
            TextBox1.Text = "Computer Prediction: O is winning."
        Else
            TextBox1.Text = "Computer Prediction: Currently a Draw."
        End If
        TextBox2.Text = "Searching at a depth of: " & CStr(AbsoluteDepth) & ", which took: " & watch.Elapsed.TotalSeconds & " seconds." & Environment.NewLine
        'TextBox2.Text = CStr(StoredData(9)) & Environment.NewLine
        For i = 0 To 8
            TextBox2.Text &= CStr(StoredData(i)) & Environment.NewLine
        Next

        'Checks for end positions.
        DisplayWin(CheckWin(table))

        ProgressBar1.Visible = False
    End Sub

    Public Function MiniMax(Board(,) As String, Depth As Integer, isMax As Boolean, Alpha As Decimal, Beta As Decimal) As Decimal
        Dim Score As Decimal
        Dim BestScore As Decimal
        If Depth > AbsoluteDepth Then AbsoluteDepth = Depth 'increments depth for visual purposes.

        If isMax Then 'Computer, = X
            BestScore = Integer.MinValue
            If CheckWin(Board) <> "" Then 'if an end position has been reached, then the position is evaluated and returned up a layer.
                Return Evaluation(Depth)
            Else
                For a = 0 To 2
                    For b = 0 To 2
                        If Board(a, b) = "" Then
                            Board(a, b) = "X"
                            'For each move possible, it plays it on the temporary board and runs that position through MiniMax.
                            Score = MiniMax(Board, Depth + 1, False, Alpha, Beta) 'MiniMax called recursively for the opposite player (Human - O) with the new board position.
                            If Score > BestScore Then
                                BestScore = Score
                            End If
                            Board(a, b) = "" 'Undos move.
                            Alpha = Math.Max(Alpha, Score)
                            If Beta <= Alpha Then 'Move was too good - prune branch.
                                Return BestScore
                            End If
                        End If
                    Next
                Next
                Return BestScore 'At the conclusion of the search, MiniMax returns its chosen move's score up a layer.
            End If

        Else 'Human, O.
            BestScore = Integer.MaxValue
            If CheckWin(Board) <> "" Then 'if an end position has been reached, then the position is evaluated and returned up a layer.
                Return Evaluation(Depth)
            Else
                For a = 0 To 2
                    For b = 0 To 2
                        If Board(a, b) = "" Then
                            Board(a, b) = "O"
                            'For each move possible, it plays it on the temporary board and runs that position through MiniMax.
                            Score = MiniMax(Board, Depth + 1, True, Alpha, Beta) 'MiniMax called recursively for the opposite player (Human - O) with the new board position.
                            If Score < BestScore Then
                                BestScore = Score
                            End If
                            Board(a, b) = "" 'Undos move.
                            Beta = Math.Min(Beta, Score)
                            If Beta <= Alpha Then 'Move was too good - prune branch.
                                Return BestScore
                                Exit Function
                            End If
                        End If
                    Next
                Next
                Return BestScore 'At the conclusion of the search, MiniMax returns its chosen move's score up a layer.
            End If
        End If
    End Function

    Function Evaluation(ByRef Depth As Integer) As Decimal
        Dim Result As Decimal
        If CheckWin(board) = "X" Then
            Result = 1 - CDec((Depth / 10)) 'Chooses the shortest pathway to winning.
            '(faster wins are given a higher score than slower wins).
        ElseIf CheckWin(board) = "O" Then
            Result = CDec((Depth / 10)) - 1 'Chooses the longest pathway to losing.
        ElseIf CheckWin(board) = "Draw" Then
            Result = 0
        End If
        Return Result
    End Function

    Public Function CheckWin(ByRef tempboard(,) As String) As String
        winner = ""
        tempcheck = "O"
        For n = 0 To 1 'Checks wins for O first, then for X.
            For x = 0 To 2
                'Checks horizontal + vertical wins.
                If tempboard(x, 0) = tempcheck And tempboard(x, 1) = tempcheck And tempboard(x, 2) = tempcheck Then winner = tempcheck
                If tempboard(0, x) = tempcheck And tempboard(1, x) = tempcheck And tempboard(2, x) = tempcheck Then winner = tempcheck
            Next
            'Checks diagonal wins.
            If tempboard(0, 0) = tempcheck And tempboard(1, 1) = tempcheck And tempboard(2, 2) = tempcheck Then winner = tempcheck
            If tempboard(0, 2) = tempcheck And tempboard(1, 1) = tempcheck And tempboard(2, 0) = tempcheck Then winner = tempcheck
            tempcheck = "X"
        Next
        If winner = "" Then
            draw = True
            'If all the elements in the board are full and a winner hasn't been decided, then the game must be a draw.
            'Otherwise, the end position has not been reached and the game continues.
            For a = 0 To 2
                For b = 0 To 2
                    If tempboard(a, b) = "" Then
                        draw = False
                        Exit For
                    End If
                Next
            Next
            If draw Then winner = "Draw"
        End If
        Return winner
    End Function

    Private Sub ResetBtn_Click(sender As Object, e As EventArgs) Handles ResetBtn.Click
        Slot0.Text = ""
        Slot1.Text = ""
        Slot2.Text = ""
        Slot3.Text = ""
        Slot4.Text = ""
        Slot5.Text = ""
        Slot6.Text = ""
        Slot7.Text = ""
        Slot8.Text = ""
        For x = 0 To 2
            For y = 0 To 2
                table(x, y) = ""
            Next
        Next
        TextBox1.Text = ""
        TextBox2.Text = ""
        PlayerChanger.Text = "O"
    End Sub

    Private Sub PlayerChanger_Click(sender As Object, e As EventArgs) Handles PlayerChanger.Click
        If PlayerChanger.Text = "O" Then
            PlayerChanger.Text = "X"
        Else
            PlayerChanger.Text = "O"
        End If
    End Sub
End Class
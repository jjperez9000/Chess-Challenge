using ChessChallenge.API;
using System;

public class MyBot : IChessBot
{
	int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };
	int maxDepth = 3;
	bool playerColor = false;
	public Move Think(Board board, Timer timer)
	{
		playerColor = board.IsWhiteToMove ? true : false;
		Move[] moves = board.GetLegalMoves();
		(Move bestMove, int bestScore) = Search(board, 0, int.MinValue, int.MaxValue, board.IsWhiteToMove);
		return bestMove;

	}

	public (Move, int) Search(Board board, int depth, int alpha, int beta, bool maximizingPlayer)
	{
		if (depth == maxDepth || board.IsInCheckmate()) // Adjust according to your end-game conditions
		{
			return (new Move(), Evaluate(board)); // Return a dummy move with the evaluation score
		}

		Move bestMove = Move.NullMove;
		Move[] moves = board.GetLegalMoves();

		// if it's our turn to move 
		if (maximizingPlayer)
		{
			int maxEval = int.MinValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);
				(_, int eval) = Search(board, depth + 1, alpha, beta, false);
				board.UndoMove(move);

				if (eval > maxEval)
				{
					maxEval = eval;
					bestMove = move;
					alpha = Math.Max(alpha, eval);
					if (beta <= alpha) break; // Beta cut-off
				}
			}
			return (bestMove, maxEval);
		}
		else
		{
			int minEval = int.MaxValue;
			foreach (Move move in moves)
			{
				board.MakeMove(move);
				(_, int eval) = Search(board, depth + 1, alpha, beta, true);
				board.UndoMove(move);

				if (eval < minEval)
				{
					minEval = eval;
					bestMove = move;
					beta = Math.Min(beta, eval);
					if (beta <= alpha) break; // Alpha cut-off
				}
			}
			return (bestMove, minEval);
		}
	}


	// Get the current state of the board. 
	public int Evaluate(Board board)
	{
		int whiteScore = 0;
		int blackScore = 0;

		if (board.IsInCheckmate())
		{
			return board.IsWhiteToMove ? int.MinValue : int.MaxValue;
		}
		for (int i = 0; i < 64; i++)
		{
			Piece piece = board.GetPiece(new Square(i));
			if (piece.IsWhite)
			{
				whiteScore += pieceValues[(int)piece.PieceType];
			}
			else if (!piece.IsNull)
			{
				blackScore += pieceValues[(int)piece.PieceType];
			}
		}
		return whiteScore - blackScore;
	}
}



// public (Move, int) Search(Board board, int depth, int alpha, int beta, bool maximizingPlayer)
// {
// 	if (depth == maxDepth || board.IsInCheckmate()) // Adjust according to your end-game conditions
// 	{
// 		return (new Move(), Evaluate(board)); // Return a dummy move with the evaluation score
// 	}

// 	Move bestMove = Move.NullMove;
// 	Move[] moves = board.GetLegalMoves();

// 	if (maximizingPlayer)
// 	{
// 		int maxEval = int.MinValue;
// 		foreach (Move move in moves)
// 		{
// 			board.MakeMove(move);
// 			(_, int eval) = Search(board, depth + 1, alpha, beta, false);
// 			board.UndoMove(move);

// 			if (eval > maxEval)
// 			{
// 				maxEval = eval;
// 				bestMove = move;
// 				alpha = Math.Max(alpha, eval);
// 				if (beta <= alpha) break; // Beta cut-off
// 			}
// 		}
// 		return (bestMove, maxEval);
// 	}
// 	else
// 	{
// 		int minEval = int.MaxValue;
// 		foreach (Move move in moves)
// 		{
// 			board.MakeMove(move);
// 			(_, int eval) = Search(board, depth + 1, alpha, beta, true);
// 			board.UndoMove(move);

// 			if (eval < minEval)
// 			{
// 				minEval = eval;
// 				bestMove = move;
// 				beta = Math.Min(beta, eval);
// 				if (beta <= alpha) break; // Alpha cut-off
// 			}
// 		}
// 		return (bestMove, minEval);
// 	}
// }

// if (depth == maxDepth || board.IsInCheckmate()) // Adjust according to your end-game conditions
// 		{
// 			return (new Move(), Evaluate(board)); // Return a dummy move with the evaluation score
// 		}

// 		Move[] moves = board.GetLegalMoves();
// 		Move bestMove = Move.NullMove;

// 		int eval = maximizingPlayer ? int.MinValue : int.MaxValue;
// 		int bestEval = maximizingPlayer ? int.MinValue : int.MaxValue;

// 		foreach (Move move in moves)
// 		{
// 			board.MakeMove(move);
// 			(_, int currentEval) = Search(board, depth + 1, alpha, beta, !maximizingPlayer);
// 			board.UndoMove(move);

// 			eval = maximizingPlayer ? Math.Max(eval, currentEval) : Math.Min(eval, currentEval);
// 			bestMove = eval == currentEval ? move : bestMove;

// 			if (maximizingPlayer)
// 			{
// 				alpha = Math.Max(alpha, eval);
// 				if (beta <= alpha)
// 					break; // Beta cut-off
// 			}
// 			else
// 			{
// 				beta = Math.Min(beta, eval);
// 				if (beta <= alpha)
// 					break; // Alpha cut-off
// 			}
// 		}

// 		return (bestMove, eval);
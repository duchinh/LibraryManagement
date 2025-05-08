interface MostBorrowedBook {
  title: string;
  borrowCount: number;
}

interface UserActivity {
  userName: string;
  requestsMade: number;
  booksBorrowed: number;
  requestsDone: number;
  requestsPending: number;
  requestsRejected: number;
}

export interface DashboardData {
  totalBooks: number;
  totalBorrowedBooks: number;
  totalUsers: number;
  totalBorrowingRequests: number;
  mostBorrowedBooks: MostBorrowedBook[];
  userActivities: UserActivity[];
}

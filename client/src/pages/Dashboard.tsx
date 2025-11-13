import { useOktaAuth } from '@okta/okta-react';

export default function Dashboard() {
  const { oktaAuth, authState } = useOktaAuth();

  const handleLogout = async () => {
    await oktaAuth.signOut();
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <h1 className="text-xl font-bold text-gray-900">HumbleDesk</h1>
            </div>
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-700">
                {authState?.idToken?.claims.email || 'User'}
              </span>
              <button
                onClick={handleLogout}
                className="px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </nav>

      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div className="px-4 py-6 sm:px-0">
          <div className="bg-white rounded-lg shadow p-6">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Welcome to HumbleDesk</h2>
            <p className="text-gray-600 mb-4">
              AI-Powered Ticketing System for efficient support management.
            </p>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div className="border rounded-lg p-4">
                <h3 className="font-semibold text-lg mb-2">My Tickets</h3>
                <p className="text-gray-600 text-sm">View and manage your support tickets</p>
              </div>
              <div className="border rounded-lg p-4">
                <h3 className="font-semibold text-lg mb-2">Create Ticket</h3>
                <p className="text-gray-600 text-sm">Submit new support requests with AI assistance</p>
              </div>
              <div className="border rounded-lg p-4">
                <h3 className="font-semibold text-lg mb-2">Knowledge Base</h3>
                <p className="text-gray-600 text-sm">Browse solutions and common issues</p>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}

import { useOktaAuth } from '@okta/okta-react';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../lib/api';

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
              <h1 className="text-xl font-bold text-gray-900">COI Management System</h1>
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
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Welcome to COI System</h2>
            <p className="text-gray-600 mb-4">
              This is the Conflict of Interest Management System dashboard.
            </p>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div className="border rounded-lg p-4">
                <h3 className="font-semibold text-lg mb-2">My Assignments</h3>
                <p className="text-gray-600 text-sm">View and complete your COI disclosures</p>
              </div>
              <div className="border rounded-lg p-4">
                <h3 className="font-semibold text-lg mb-2">Question Builder</h3>
                <p className="text-gray-600 text-sm">Create and manage questionnaires</p>
              </div>
              <div className="border rounded-lg p-4">
                <h3 className="font-semibold text-lg mb-2">Review Queue</h3>
                <p className="text-gray-600 text-sm">Process submissions awaiting review</p>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}

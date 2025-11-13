import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Security, LoginCallback, SecureRoute } from '@okta/okta-react';
import { OktaAuth, toRelativeUrl } from '@okta/okta-auth-js';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { oktaConfig } from './config/okta.config';
import Dashboard from './pages/Dashboard';
import Login from './pages/Login';

const oktaAuth = new OktaAuth(oktaConfig);
const queryClient = new QueryClient();

function App() {
  const restoreOriginalUri = async (_oktaAuth: OktaAuth, originalUri: string) => {
    window.location.replace(toRelativeUrl(originalUri || '/', window.location.origin));
  };

  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <Security oktaAuth={oktaAuth} restoreOriginalUri={restoreOriginalUri}>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/login/callback" element={<LoginCallback />} />
            <Route
              path="/"
              element={
                <SecureRoute>
                  <Dashboard />
                </SecureRoute>
              }
            />
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Security>
      </Router>
    </QueryClientProvider>
  );
}

export default App;

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Define a custom metric to track the full background processing time
const workflowDuration = new Trend('workflow_e2e_duration');

export const options = {
  stages: [
    { duration: '30s', target: 5 },  // Ramp to 5 users
    { duration: '1m',  target: 5 },  // Hold for 1 minute
    { duration: '10s', target: 0 },  // Ramp down
  ],
};

const PRODUCT_IDS = [
  "0e5d9b8b-c30d-4d53-8298-a39e0acadcfc", 
  "15d94488-db9b-4767-9e92-d6328b735e0e", 
  "4d76bc3a-168d-4054-8154-6f6246355e53", 
];

export default function () {
  // PHASE 1: Trigger the Workflow
  const url = 'http://localhost:3200/api/orders';
  const randomProduct = PRODUCT_IDS[Math.floor(Math.random() * PRODUCT_IDS.length)];
  
  const payload = JSON.stringify({
    orderItems: [{ productId: randomProduct, quantity: 1 }]
  });

  const params = { headers: { 'Content-Type': 'application/json' } };

  // Start the timer
  const startTime = new Date();

  const res = http.post(url, payload, params);

  const success = check(res, {
    'API responded 202': (r) => r.status === 202,
    'Has instance ID': (r) => r.json('workflowInstanceId') !== undefined,
  });

  if (!success) {
    sleep(1);
    return; // Skip polling if the order failed to start
  }

  const instanceId = res.json('workflowInstanceId');
  const statusUrl = `http://localhost:3200/api/orders/status/${instanceId}`;

  // PHASE 2: Poll until Complete
  let isComplete = false;
  let attempts = 0;
  const maxAttempts = 20; // Prevent infinite loops (approx 20 seconds timeout)

  while (!isComplete && attempts < maxAttempts) {
    sleep(1); // Wait 1 second between checks
    attempts++;

    const pollRes = http.get(statusUrl);
    
    // Check if the workflow is done
    const status = pollRes.json('runtimeStatus'); 
    
    if (status === 'Completed' || status === 'Failed' || status === 'Terminated') {
      isComplete = true;
      const endTime = new Date();
      
      // Calculate total duration in milliseconds and add to our custom metric
      workflowDuration.add(endTime - startTime);
      
      check(pollRes, {
        'Workflow completed successfully': (r) => r.json('runtimeStatus') === 'Completed',
      });
    }
  }

  // Mark as failed if it timed out
  check(attempts, {
    'Workflow finished within timeout': (a) => a < maxAttempts,
  });
}
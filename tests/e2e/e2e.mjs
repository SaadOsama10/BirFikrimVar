// End-to-end checks (Playwright) for a freshly seeded instance: Seed__DemoData=true plus an admin account.
// It publishes/rejects the seeded pending posts, so run it against a throwaway database.
import { chromium } from 'playwright';
const B = process.env.BASE_URL || 'http://localhost:5080';
const DEMO_PW = process.env.DEMO_PASSWORD, ADMIN_EMAIL = process.env.ADMIN_EMAIL || 'admin@example.com', ADMIN_PW = process.env.ADMIN_PASSWORD;
if (!DEMO_PW || !ADMIN_PW) throw new Error('Set DEMO_PASSWORD and ADMIN_PASSWORD (the Seed__* values of the instance under test).');
const results = [];
const ok = (name, cond, extra='') => { results.push([cond?'PASS':'FAIL', name, extra]); };
const browser = await chromium.launch();
const errors = [];
async function ctx() { const c = await browser.newContext(); const p = await c.newPage(); p.on('console', m => { if (m.type()==='error') errors.push(m.text()); }); return [c,p]; }
async function login(p, email, pw) {
  await p.goto(B + '/Account/Login'); await p.fill('#Email', email); await p.fill('#Password', pw);
  await Promise.all([p.waitForNavigation(), p.click('input[type=submit]')]);
}
// anonymous
let [c1, p1] = await ctx();
await p1.goto(B + '/');
ok('home has 6 published cards', (await p1.locator('.post-card').count()) === 6, String(await p1.locator('.post-card').count()));
ok('pending post not on home', !(await p1.content()).includes('Peer Tutoring'));
let r = await p1.goto(B + '/Admin/ManagePost'); ok('anon admin -> login redirect', p1.url().includes('/Account/Login'), p1.url());
r = await p1.goto(B + '/Comment/Inbox/1'); ok('anon inbox -> login', p1.url().includes('/Account/Login'));
r = await p1.goto(B + '/Post/Details/7'); ok('pending details anon 404', r.status() === 404, String(r.status()));
await p1.goto(B + '/Home/Search?keywords=Peer'); ok('search hides pending', !(await p1.content()).includes('Peer Tutoring'));
await p1.goto(B + '/Home/Search?keywords=study'); ok('search finds published', (await p1.content()).includes('Study-Buddy'));
// language
await p1.goto(B + '/'); await p1.click('#lang-ar'); await p1.waitForLoadState();
ok('AR sets rtl', (await p1.getAttribute('html','dir')) === 'rtl' && (await p1.getAttribute('html','lang')) === 'ar');
await p1.click('#lang-en'); await p1.waitForLoadState(); ok('EN sets ltr', (await p1.getAttribute('html','dir')) === 'ltr');
// demo user
let [c2, p2] = await ctx();
await login(p2, 'demo@birfikrimvar.app', DEMO_PW);
ok('demo login', (await p2.content()).includes('Hello'));
await p2.goto(B + '/Post/Details/1');
const before = parseInt(await p2.textContent('#like-count'));
await p2.click('#btn-like'); await p2.waitForLoadState();
const after = parseInt(await p2.textContent('#like-count'));
ok('like increments', after === before + 1, `${before}->${after}`);
await p2.click('#btn-like'); await p2.waitForLoadState();
ok('unlike decrements', parseInt(await p2.textContent('#like-count')) === before);
r = await p2.goto(B + '/Like/Toggle/1'); ok('GET like rejected (405)', r.status() === 405, String(r.status()));
r = await p2.goto(B + '/Admin/ManagePost'); ok('non-admin forbidden', r.status() === 403 || p2.url().includes('AccessDenied'), p2.url() + ' ' + r.status());
r = await p2.goto(B + '/Comment/Inbox/1'); ok('non-author inbox forbidden', p2.url().includes('AccessDenied') || r.status()===403, p2.url());
// create: invalid file
await p2.goto(B + '/Post/Create');
await p2.fill('#Title', 'Demo submitted idea'); await p2.fill('#Tags', 'demo, test');
await p2.fill('textarea[name="PostPlot[0].Text"]', 'A test section.');
await p2.setInputFiles('input[name="PostPlot[0].Image"]', 'fake.png');
await Promise.all([p2.waitForNavigation(), p2.click('input[type=submit]')]);
ok('fake image rejected', (await p2.content()).includes('Only JPG, PNG, GIF or WebP'));
await p2.goto(B + '/Post/Create');
await p2.fill('#Title', 'Demo submitted idea'); await p2.fill('#Tags', 'demo, test');
await p2.fill('textarea[name="PostPlot[0].Text"]', 'A test section.');
await p2.setInputFiles('input[name="PostPlot[0].Image"]', 'ok.png');
await p2.click('#add-plot');
await p2.fill('textarea[name="PostPlot[1].Text"]', 'Second section without image.');
await Promise.all([p2.waitForNavigation(), p2.click('input[type=submit]')]);
ok('valid post submitted pending', (await p2.content()).includes('pending approval'));
ok('new post not public yet', !(await p2.content()).includes('Demo submitted idea'));
// save / unsave
await p2.goto(B + '/');
const saveBtn = p2.locator('.post-card', { hasText: 'Study-Buddy' }).locator('button');
await saveBtn.click(); await p2.waitForLoadState();
ok('saved shows in saved list', (await (await p2.goto(B + '/Post/SavedPosts')).text()).includes('Study-Buddy'));
// admin
let [c3, p3] = await ctx();
await login(p3, ADMIN_EMAIL, ADMIN_PW);
await p3.goto(B + '/Admin/ManagePost');
ok('admin sees queue incl. new post', (await p3.content()).includes('Demo submitted idea') && (await p3.content()).includes('Peer Tutoring'));
await p3.click('tr:has-text("Demo submitted idea") a'); await p3.waitForLoadState();
await Promise.all([p3.waitForNavigation(), p3.click('#btn-publish')]);
ok('publish flash', (await p3.content()).includes('published successfully'));
await p1.goto(B + '/'); ok('published post now public', (await p1.content()).includes('Demo submitted idea'));
await p3.goto(B + '/Admin/ManagePost'); await p3.click('tr:has-text("Peer Tutoring") a'); await p3.waitForLoadState();
await Promise.all([p3.waitForNavigation(), p3.click('#btn-reject')]);
ok('reject flash', (await p3.content()).includes('rejected successfully'));
await p1.goto(B + '/'); ok('rejected stays hidden', !(await p1.content()).includes('Peer Tutoring'));
// uploaded image served
for (const [s,n,e] of results) console.log(s, n, e);
console.log('console errors:', JSON.stringify(errors.slice(0,5)));
await browser.close();
if (results.some(r => r[0] === 'FAIL')) process.exit(1);
